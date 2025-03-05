using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Text;

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
    }


    public interface ICacheService
    {
        T Get<T>(string key);
        bool Set<T>(string key, T value, TimeSpan expiration);
        object Remove(string key);
        bool Exist(string key);
    }
}