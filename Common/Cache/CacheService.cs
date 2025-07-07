using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Text;
using System.Threading.Tasks;
namespace Common.Cache;
public class CacheService : ICacheService
{
    private readonly IDatabase _redisCache;
    private readonly IMemoryCache _memoryCache;
    private readonly IConnectionMultiplexer _redisConnection;

    private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    public CacheService(IConnectionMultiplexer redisConnection, IMemoryCache memoryCache)
    {
        _redisConnection = redisConnection;
        _redisCache = redisConnection.GetDatabase();
        _memoryCache = memoryCache;
    }

    #region Async Methods

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

        if (_memoryCache.TryGetValue(key, out T value))
        {
            return value;
        }

        return default;
    }

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
            _memoryCache.Set(key, value, expiration);
            return true;
        }
    }

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

    #endregion

    #region Sync Methods

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

        if (_memoryCache.TryGetValue(key, out T value))
        {
            return value;
        }

        return default;
    }

    public bool Set<T>(string key, T value, TimeSpan expiration)
    {
        try
        {
            return _redisCache.StringSet(key, Serialize(value), expiration);
        }
        catch
        {
            _memoryCache.Set(key, value, expiration);
            return true;
        }
    }

    public object Remove(string key)
    {
        bool deletedFromRedis = false;
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
    Task<T> GetAsync<T>(string key);
    Task<bool> SetAsync<T>(string key, T value, TimeSpan expiration);
    Task<bool> RemoveAsync(string key);
    Task<bool> ExistAsync(string key);
    T Get<T>(string key);
    bool Set<T>(string key, T value, TimeSpan expiration);
    object Remove(string key);
    bool Exist(string key);
}