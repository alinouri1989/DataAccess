using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IDatabase _redisCache;
        private readonly IMemoryCache _memoryCache;

        public CacheService(IConnectionMultiplexer redisConnection, IMemoryCache memoryCache)
        {
            _redisConnection = redisConnection;
            _redisCache = redisConnection.GetDatabase();
            _memoryCache = memoryCache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (_redisConnection.IsConnected)
            {
                var redisValue = await _redisCache.StringGetAsync(key);
                if (!redisValue.IsNullOrEmpty)
                {
                    return Deserialize<T>(redisValue);
                }
            }

            // Fallback to memory cache if Redis is not available  
            if (_memoryCache.TryGetValue(key, out T memoryValue))
            {
                return memoryValue;
            }

            return default;
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            if (_redisConnection.IsConnected)
            {
                return await _redisCache.StringSetAsync(key, Serialize(value), expiration);
            }
            else
            {
                _memoryCache.Set(key, value, expiration);
                return true;
            }
        }

        public async Task<bool> RemoveAsync(string key)
        {
            if (_redisConnection.IsConnected)
            {
                bool isKeyExist = await _redisCache.KeyExistsAsync(key);
                if (isKeyExist)
                {
                    return await _redisCache.KeyDeleteAsync(key);
                }
            }
            else
            {
                _memoryCache.Remove(key);
                return true;
            }
            return false;
        }

        public async Task<bool> ExistAsync(string key)
        {
            if (_redisConnection.IsConnected)
            {
                return await _redisCache.KeyExistsAsync(key);
            }
            else
            {
                return _memoryCache.TryGetValue(key, out _);
            }
        }

        private byte[] Serialize<T>(T value)
        {
            var serializedValue = JsonConvert.SerializeObject(value);
            return Encoding.UTF8.GetBytes(serializedValue);
        }

        private T Deserialize<T>(byte[] value)
        {
            var serializedValue = Encoding.UTF8.GetString(value);
            return JsonConvert.DeserializeObject<T>(serializedValue);
        }
        public T Get<T>(string key)
        {
            if (_redisConnection.IsConnected)
            {
                var redisValue = _redisCache.StringGet(key);
                if (!redisValue.IsNullOrEmpty)
                {
                    return Deserialize<T>(redisValue);
                }
            }
            else
            {
                if (_memoryCache.TryGetValue(key, out T memoryValue))
                {
                    return memoryValue;
                }
            }
            return default;
        }

        public bool Set<T>(string key, T value, TimeSpan expiration)
        {
            if (_redisConnection.IsConnected)
            {
                return _redisCache.StringSet(key, Serialize(value), expiration);
            }
            else
            {
                _memoryCache.Set(key, value, expiration);
                return true;
            }
        }

        public object Remove(string key)
        {
            if (_redisConnection.IsConnected)
            {
                bool _isKeyExist = _redisCache.KeyExists(key);
                if (_isKeyExist == true)
                {
                    return _redisCache.KeyDelete(key);
                }
            }
            else
            {
                _memoryCache.Remove(key);
                return true;
            }
            return false;
        }

        public bool Exist(string key)
        {
            if (_redisConnection.IsConnected)
            {
                return _redisCache.KeyExists(key);
            }
            else
            {
                return _memoryCache.TryGetValue(key, out _);
            }
        }

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
}