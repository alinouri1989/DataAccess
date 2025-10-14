using Common.Caching.Models;
using Common.Security.Crypto;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;

namespace Common.Caching;
public class CacheService : ICacheService
{
    #region Fields
    private readonly IDatabase _redisCache;
    private readonly IMemoryCache _memoryCache;
    private readonly IConnectionMultiplexer _redisConnection;
    private readonly IEncryptor _encryptor;
    private readonly bool _enableTtlRetrieval;
    // دیکشنری ترد_سیف برای نگهداری زمان انقضای کلیدهای مموری_کش
    private readonly ConcurrentDictionary<string, DateTime> _memoryExpirationTracker;
    private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };
    #endregion

    #region Constructor
    public CacheService(
        IConnectionMultiplexer redisConnection,
        IMemoryCache memoryCache,
        IEncryptor encryptor,
        IOptions<CacheSettings> cacheSettings = null)
    {
        _redisConnection = redisConnection;
        _redisCache = redisConnection.GetDatabase();
        _memoryCache = memoryCache;
        _encryptor = encryptor;
        _enableTtlRetrieval = cacheSettings?.Value?.EnableTtlRetrieval ?? false;
        _memoryExpirationTracker = new ConcurrentDictionary<string, DateTime>();
    }
    #endregion

    #region Get

    /// <summary>
    /// دریافت مقدار از کش
    /// ابتدا از ردیس، در صورت عدم دسترسی از مموری_کش
    /// </summary>
    public async Task<T> GetAsync<T>(string key)
    {
        try
        {
            var redisValue = await _redisCache.StringGetAsync(key);
            if (!redisValue.IsNullOrEmpty)
            {
                return Deserialize<T>(redisValue);
            }
        }
        catch
        {
            // Redis در دسترس نیست، به MemoryCache می‌رویم
        }

        try
        {
            if (_memoryCache.TryGetValue(key, out byte[] value) && value != null && value.Length > 0)
            {
                return Deserialize<T>(value);
            }
        }
        catch
        {
            // خطا در دریافت از MemoryCache
        }

        // کلید یافت نشد
        return default;
    }

    /// <summary>
    /// دریافت مقدار از کش 
    /// </summary>
    public T Get<T>(string key)
    {
        try
        {
            var redisValue = _redisCache.StringGet(key);
            if (!redisValue.IsNullOrEmpty)
            {
                return Deserialize<T>(redisValue);
            }
        }
        catch
        {
            // Redis در دسترس نیست
        }

        try
        {
            if (_memoryCache.TryGetValue(key, out byte[] value) && value != null && value.Length > 0)
            {
                return Deserialize<T>(value);
            }
        }
        catch
        {
            // خطا در دریافت
        }

        return default;
    }

    /// <summary>
    /// دریافت و رمزگشایی مقدار از کش
    /// </summary>
    public async Task<T> CipherGetAsync<T>(string key, string password)
    {
        // دریافت داده رمزشده
        var encryptedData = await GetAsync<string>(key);

        if (string.IsNullOrEmpty(encryptedData))
        {
            return default;
        }

        try
        {
            // رمزگشایی
            var decryptedJson = _encryptor.Decrypt(encryptedData, password);
            return JsonConvert.DeserializeObject<T>(decryptedJson, _serializerSettings);
        }
        catch
        {
            // خطا در رمزگشایی
            return default;
        }
    }

    #endregion

    #region Set

    /// <summary>
    /// ذخیره مقدار در کش با زمان انقضا 
    /// ابتدا در ردیس، در صورت عدم دسترسی در مموری_کش
    /// </summary>
    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        try
        {
            var serializedData = Serialize(value);

            var result = await _redisCache.StringSetAsync(key, serializedData, expiration);
            return result;
        }
        catch
        {
            try
            {
                SetInMemoryCache(key, Serialize(value), expiration);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// ذخیره مقدار در کش با زمان انقضا
    /// </summary>
    public bool Set<T>(string key, T value, TimeSpan expiration)
    {
        try
        {
            var serializedData = Serialize(value);
            return _redisCache.StringSet(key, serializedData, expiration);
        }
        catch
        {
            // فالبک به مموری_کش
            try
            {
                SetInMemoryCache(key, Serialize(value), expiration);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// رمزنگاری و ذخیره مقدار در کش
    /// </summary>
    public async Task<bool> CipherSetAsync<T>(string key, T value, string password, TimeSpan expiration)
    {
        try
        {
            // سریالایز
            var json = JsonConvert.SerializeObject(value, _serializerSettings);

            // رمزنگاری
            var encryptedData = _encryptor.Encrypt(json, password);

            // ذخیره
            return await SetAsync(key, encryptedData, expiration);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// ذخیره در مموری_کش با ردیابی زمان انقضا
    /// </summary>
    private void SetInMemoryCache(string key, byte[] value, TimeSpan expiration)
    {
        // تنظیمات ذخیره‌سازی
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        // ثبت فالبک برای حذف خودکار از ترکر
        cacheOptions.RegisterPostEvictionCallback((evictedKey, evictedValue, reason, state) =>
        {
            if (evictedKey != null)
            {
                // حذف از ترکر وقتی از مموری_کش حذف شد
                _memoryExpirationTracker.TryRemove(evictedKey.ToString(), out _);
            }
        });

        // ذخیره در مموری_کش
        _memoryCache.Set(key, value, cacheOptions);

        // ذخیره زمان انقضا در ترکر فقط اگر فعال باشد
        if (_enableTtlRetrieval)
        {
            var expirationTime = DateTime.UtcNow.Add(expiration);
            _memoryExpirationTracker.AddOrUpdate(
                key,
                expirationTime,
                (k, oldValue) => expirationTime
            );
        }
    }

    #endregion

    #region Remove

    /// <summary>
    /// حذف کلید از کش
    /// </summary>
    public async Task<bool> RemoveAsync(string key)
    {
        bool deletedFromRedis = false;

        try
        {
            deletedFromRedis = await _redisCache.KeyDeleteAsync(key);
        }
        catch
        {
        }

        bool deletedFromMemory = false;

        try
        {
            _memoryCache.Remove(key);
            _memoryExpirationTracker.TryRemove(key, out _);
            deletedFromMemory = true;
        }
        catch
        {
        }

        return deletedFromRedis || deletedFromMemory;
    }

    /// <summary>
    /// حذف کلید از کش
    /// </summary>
    public bool Remove(string key)
    {
        bool deletedFromRedis = false;

        try
        {
            deletedFromRedis = _redisCache.KeyDelete(key);
        }
        catch
        {
        }

        try
        {
            _memoryCache.Remove(key);
            _memoryExpirationTracker.TryRemove(key, out _);
            return true;
        }
        catch
        {
            return deletedFromRedis;
        }
    }

    #endregion

    #region Exist

    /// <summary>
    /// بررسی وجود کلید در کش
    /// </summary>
    public async Task<bool> ExistAsync(string key)
    {
        // بررسی در Redis
        try
        {
            return await _redisCache.KeyExistsAsync(key);
        }
        catch
        {
            // اگر ردیس در دسترس نبود، مموری_کش را بررسی می‌کنیم
            return _memoryCache.TryGetValue(key, out _);
        }
    }

    /// <summary>
    /// بررسی وجود کلید در کش 
    /// </summary>
    public bool Exist(string key)
    {
        try
        {
            return _redisCache.KeyExists(key);
        }
        catch
        {
            return _memoryCache.TryGetValue(key, out _);
        }
    }

    #endregion

    #region TTL

    /// <summary>
    /// دریافت زمان انقضای باقیمانده کلید
    /// </summary>
    /// <param name="key">کلید مورد نظر</param>
    /// <returns>
    /// زمان باقیمانده تا انقضا
    /// null اگر کلید وجود نداشته باشد یا قابلیت غیرفعال باشد
    /// </returns>
    public async Task<TimeSpan?> GetTtlAsync(string key)
    {
        // بررسی فعال بودن قابلیت
        if (!_enableTtlRetrieval)
        {
            return null;
        }

        // ابتدا Redis را بررسی می‌کنیم
        try
        {
            var ttl = await _redisCache.KeyTimeToLiveAsync(key);
            if (ttl.HasValue)
            {
                return ttl.Value;
            }
        }
        catch
        {
            // Redis در دسترس نیست
        }

        // بررسی مموری_کش
        if (_memoryCache.TryGetValue(key, out _))
        {
            // دریافت زمان انقضا از ترکر
            if (_memoryExpirationTracker.TryGetValue(key, out DateTime expirationTime))
            {
                // محاسبه زمان باقیمانده
                var remainingTime = expirationTime - DateTime.UtcNow;

                // اگر مثبت بود هنوز منقضی نشده
                return remainingTime > TimeSpan.Zero ? remainingTime : null;
            }
        }

        return null;
    }

    /// <summary>
    /// دریافت زمان انقضای باقیمانده کلید (sync)
    /// </summary>
    public TimeSpan? GetTtl(string key)
    {
        if (!_enableTtlRetrieval)
        {
            return null;
        }

        // بررسی Redis
        try
        {
            var ttl = _redisCache.KeyTimeToLive(key);
            if (ttl.HasValue)
            {
                return ttl.Value;
            }
        }
        catch
        {
            // Redis در دسترس نیست
        }

        // بررسی MemoryCache
        if (_memoryCache.TryGetValue(key, out _))
        {
            if (_memoryExpirationTracker.TryGetValue(key, out DateTime expirationTime))
            {
                var remainingTime = expirationTime - DateTime.UtcNow;
                return remainingTime > TimeSpan.Zero ? remainingTime : null;
            }
        }

        return null;
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// سریالایز object به byte array
    /// </summary>
    private byte[] Serialize<T>(T value)
    {
        var json = JsonConvert.SerializeObject(value, _serializerSettings);
        return Encoding.UTF8.GetBytes(json);
    }

    /// <summary>
    /// دی‌سریالایز byte array به object
    /// </summary>
    private T Deserialize<T>(byte[] value)
    {
        var json = Encoding.UTF8.GetString(value);
        return JsonConvert.DeserializeObject<T>(json, _serializerSettings);
    }
    #endregion
}

public interface ICacheService
{
    #region Get
    Task<T> GetAsync<T>(string key);
    T Get<T>(string key);
    Task<T> CipherGetAsync<T>(string key, string password);
    #endregion

    #region Set
    Task<bool> SetAsync<T>(string key, T value, TimeSpan expiration);
    bool Set<T>(string key, T value, TimeSpan expiration);
    Task<bool> CipherSetAsync<T>(string key, T value, string password, TimeSpan expiration);
    #endregion

    #region Remove
    Task<bool> RemoveAsync(string key);
    bool Remove(string key);
    #endregion

    #region Exist
    Task<bool> ExistAsync(string key);
    bool Exist(string key);
    #endregion

    #region TTL
    Task<TimeSpan?> GetTtlAsync(string key);
    TimeSpan? GetTtl(string key);
    #endregion
}