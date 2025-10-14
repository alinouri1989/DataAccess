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
    private readonly IServiceProvider _serviceProvider;
    private readonly IEncryptor _encryptor;
    private readonly bool _enableTtlRetrieval;
    private readonly ConcurrentDictionary<string, DateTime> _expirationTracker; // برای MemoryCache
    private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };
    #endregion

    #region Ctor
    public CacheService(IConnectionMultiplexer redisConnection,
        IMemoryCache memoryCache,
        IEncryptor encryptor,
        IOptions<CacheSettings> cacheSettings = null)
    {
        _redisConnection = redisConnection;
        _redisCache = redisConnection.GetDatabase();
        _memoryCache = memoryCache;
        _encryptor = encryptor;
        _enableTtlRetrieval = cacheSettings != null ? cacheSettings.Value.EnableTtlRetrieval : false;
        _expirationTracker = new ConcurrentDictionary<string, DateTime>();
    }
    #endregion

    #region Get
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
            // Redis failed; fall back to memory cache
        }

        try
        {
            if (_memoryCache.TryGetValue(key, out byte[] value)
                && value != null && value.Length > 0)
            {
                return Deserialize<T>(value);
            }
        }
        catch
        {
        }

        return default;
    }
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
        catch { }

        try
        {
            if (_memoryCache.TryGetValue(key, out byte[] value)
                && value != null && value.Length > 0)
            {
                return Deserialize<T>(value);
            }
        }
        catch
        {
        }

        return default;
    }
    public async Task<T> CipherGetAsync<T>(string key, string password)
    {
        // بازیابی داده رمزشده از کش
        var encryptedData = await GetAsync<string>(key);

        // بررسی وجود داده
        if (string.IsNullOrEmpty(encryptedData))
        {
            return default;
        }

        try
        {
            // رمزگشایی داده
            var decryptedJson = _encryptor.Decrypt(encryptedData, password);
            return JsonConvert.DeserializeObject<T>(decryptedJson, _serializerSettings);
        }
        catch
        {
            // در صورت خطا در رمزگشایی
            return default;
        }
    }
    #endregion

    #region TTL (Time To Live)
    /// <summary>
    /// دریافت زمان انقضای باقیمانده کلید
    /// </summary>
    /// <param name="key">کلید مورد نظر</param>
    /// <returns>زمان باقیمانده تا انقضا یا null در صورت عدم وجود</returns>
    public async Task<TimeSpan?> GetTtlAsync(string key)
    {
        if (!_enableTtlRetrieval)
        {
            throw new InvalidOperationException("TTL retrieval is disabled. Enable it in CacheSettings.");
        }

        // بررسی Redis
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
            // Redis failed, continue to memory cache
        }

        // بررسی MemoryCache
        if (_memoryCache.TryGetValue(key, out _))
        {
            if (_expirationTracker.TryGetValue(key, out DateTime expirationTime))
            {
                var remainingTime = expirationTime - DateTime.UtcNow;
                return remainingTime > TimeSpan.Zero ? remainingTime : null;
            }
        }

        return null;
    }

    /// <summary>
    /// دریافت زمان انقضای باقیمانده کلید (نسخه همزمان)
    /// </summary>
    public TimeSpan? GetTtl(string key)
    {
        if (!_enableTtlRetrieval)
        {
            throw new InvalidOperationException("TTL retrieval is disabled. Enable it in CacheSettings.");
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
            // Redis failed, continue to memory cache
        }

        // بررسی MemoryCache
        if (_memoryCache.TryGetValue(key, out _))
        {
            if (_expirationTracker.TryGetValue(key, out DateTime expirationTime))
            {
                var remainingTime = expirationTime - DateTime.UtcNow;
                return remainingTime > TimeSpan.Zero ? remainingTime : null;
            }
        }

        return null;
    }
    #endregion

    #region Set
    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        try
        {
            var result = await _redisCache.StringSetAsync(key, Serialize(value), expiration);
            return result;
        }
        catch
        {
            // Fallback to memory cache
            //_memoryCache.Set(key, Serialize(value), expiration);
            SetInMemoryCache(key, Serialize(value), expiration);
            return true;
        }
    }
    public bool Set<T>(string key, T value, TimeSpan expiration)
    {
        try
        {
            return _redisCache.StringSet(key, Serialize(value), expiration);
        }
        catch
        {
            //_memoryCache.Set(key, Serialize(value), expiration);
            SetInMemoryCache(key, Serialize(value), expiration);
            return true;
        }
    }
    public async Task<bool> CipherSetAsync<T>(string key, T value, string password, TimeSpan expiration)
    {
        try
        {
            // تبدیل به JSON و رمزنگاری
            var json = JsonConvert.SerializeObject(value, _serializerSettings);
            var encryptedData = _encryptor.Encrypt(json, password);

            // ذخیره داده رمزشده
            return await SetAsync(key, encryptedData, expiration);
        }
        catch
        {
            return false;
        }
    }

    private void SetInMemoryCache(string key, byte[] value, TimeSpan expiration)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        // ثبت callback برای حذف از tracker
        options.RegisterPostEvictionCallback((evictedKey, evictedValue, reason, state) =>
        {
            _expirationTracker.TryRemove(evictedKey.ToString(), out _);
        });

        _memoryCache.Set(key, value, options);

        // ذخیره زمان انقضا در tracker
        if (_enableTtlRetrieval)
        {
            _expirationTracker[key] = DateTime.UtcNow.Add(expiration);
        }
    }
    #endregion

    #region Remove
    public async Task<bool> RemoveAsync(string key)
    {
        bool deletedFromRedis = false;
        try
        {
            deletedFromRedis = await _redisCache.KeyDeleteAsync(key);
        }
        catch { /* ignore */ }

        var deletedFromMemory = false;
        try
        {
            _memoryCache.Remove(key);
            deletedFromMemory = true;
        }
        catch { /* ignore */ }

        return deletedFromRedis || deletedFromMemory;
    }
    public object Remove(string key)
    {
        var deletedFromRedis = false;
        try
        {
            deletedFromRedis = _redisCache.KeyDelete(key);
        }
        catch { }

        try
        {
            _memoryCache.Remove(key);
            return true;
        }
        catch
        {
            return deletedFromRedis;
        }
    }
    #endregion

    #region Exist
    public async Task<bool> ExistAsync(string key)
    {
        try
        {
            return await _redisCache.KeyExistsAsync(key);
        }
        catch
        {
            return _memoryCache.TryGetValue(key, out _);
        }
    }

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

    #region Helpers
    private byte[] Serialize<T>(T value)
    {
        var serializedValue = JsonConvert.SerializeObject(value, _serializerSettings);
        return Encoding.UTF8.GetBytes(serializedValue);
    }

    private T Deserialize<T>(byte[] value)
    {
        var serializedValue = Encoding.UTF8.GetString(value);
        return JsonConvert.DeserializeObject<T>(serializedValue, _serializerSettings);
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
    object Remove(string key);
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