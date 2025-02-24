using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using System.Collections.Generic; // Ensure you're using the correct namespace  

namespace DataAccess.EFCoreSecondLevelCacheInterceptor
{
    public class EFStackExchangeCacheServiceProvider : IEFCacheServiceProvider
    {
        private readonly IReaderWriterLockProvider _readerWriterLockProvider;
        private readonly IRedisDatabase _redisCacheDatabase; // MongoDatabase or appropriate interface  
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public EFStackExchangeCacheServiceProvider(
            IReaderWriterLockProvider readerWriterLockProvider,
            IRedisDatabase redisCacheDatabase, // Adjusted to match type  
            IMemoryCache memoryCache,
            IConfiguration configuration)
        {
            _readerWriterLockProvider = readerWriterLockProvider;
            _redisCacheDatabase = redisCacheDatabase;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        /// <summary>  
        /// Clears all cached entries added by this library.  
        /// </summary>  
        public void ClearAllCachedEntries()
        {
            // Implementation for clearing cache across both Redis and Memory Cache  
            // This might need to clear all keys based on a prefix or more sophisticated logic  
            // Here we abstract it to a method that clears cache entries in the specific storage  

            if (IsConnectedRedis)
            {
                // Logic to clear Redis cache  
                // Use _redisCacheDatabase to remove keys as needed  
            }
            else
            {
                // For in-memory cache  
                _memoryCache.Dispose(); // or clear all relevant items as needed  
            }
        }

        /// <summary>  
        /// Gets a cached entry by key.  
        /// </summary>  
        public EFCachedData GetValue(EFCacheKey cacheKey, EFCachePolicy cachePolicy)
        {
            if (IsConnectedRedis)
            {
                return _readerWriterLockProvider.TryReadLocked(() =>
                    _redisCacheDatabase.GetAsync<EFCachedData>(cacheKey.KeyHash).Result); // Use Result carefully  
            }
            else
            {
                return _readerWriterLockProvider.TryReadLocked(() =>
                    _memoryCache.Get<EFCachedData>(cacheKey.KeyHash));
            }
        }

        /// <summary>  
        /// Adds a new item to the cache.  
        /// </summary>  
        public void InsertValue(EFCacheKey cacheKey, EFCachedData value, EFCachePolicy cachePolicy)
        {
            if (IsConnectedRedis)
            {
                _readerWriterLockProvider.TryWriteLocked(() =>
                {
                    // Here you need to set an item in Redis  
                    _redisCacheDatabase.AddAsync(cacheKey.KeyHash, value, cachePolicy.CacheTimeout);
                });
            }
            else
            {
                _readerWriterLockProvider.TryWriteLocked(() =>
                {
                    // Cache entry for MemoryCache  
                    _memoryCache.Set(cacheKey.KeyHash, value, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = cachePolicy.CacheTimeout // Example based on policy  
                    });
                });
            }
        }

        /// <summary>  
        /// Invalidates all of the cache entries which are dependent on any of the specified root keys.  
        /// </summary>  
        public void InvalidateCacheDependencies(EFCacheKey cacheKey)
        {
            if (IsConnectedRedis)
            {
                // Handle Redis dependencies  
                _readerWriterLockProvider.TryWriteLocked(() =>
                {
                    foreach (var dependency in cacheKey.CacheDependencies)
                    {
                        // Clear specific dependencies from Redis  
                        clearDependencyValues(dependency); // Ensure this is an awaited method if it's async  
                    }
                });
            }
            else
            {
                foreach (var dependency in cacheKey.CacheDependencies)
                {
                    _readerWriterLockProvider.TryWriteLocked(() =>
                    {
                        // Invalidate in-memory cache dependencies as necessary  
                        _memoryCache.Remove(dependency);
                    });
                }
            }
        }

        private async void clearDependencyValues(string rootCacheKey)
        {
            var dependencyKeys = await _redisCacheDatabase.GetAsync<HashSet<string>>(rootCacheKey);
            if (dependencyKeys != null)
            {
                foreach (var dependencyKey in dependencyKeys)
                {
                    await _redisCacheDatabase.RemoveAsync(dependencyKey);
                }
            }
        }

        public bool IsConnectedRedis
        {
            get
            {
                RedisConfiguration redisConfiguration = this._configuration.GetSection("Redis").Get<RedisConfiguration>();
                if (redisConfiguration == null)
                    return false;
                try
                {
                    return _redisCacheDatabase.Database.IsConnected(default, 0);
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}