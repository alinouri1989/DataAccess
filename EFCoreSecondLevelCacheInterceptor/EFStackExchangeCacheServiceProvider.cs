// Decompiled with JetBrains decompiler
// Type: EFCoreSecondLevelCacheInterceptor.EFStackExchangeCacheServiceProvider
// Assembly: DataAccess, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6804894C-F989-4432-B8EA-6F3F70ACE424
// Assembly location: C:\Users\sa.nori\AppData\Local\Temp\Riqygac\68676b643f\lib\net5.0\DataAccess.dll

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


#nullable enable
namespace EFCoreSecondLevelCacheInterceptor
{
    public class EFStackExchangeCacheServiceProvider : IEFCacheServiceProvider
    {
        private readonly
#nullable disable
        IReaderWriterLockProvider _readerWriterLockProvider;
        private readonly IRedisCacheClient _redisCacheClient;
        private readonly EFCoreSecondLevelCacheSettings _cacheSettings;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly IMemoryCacheChangeTokenProvider _signal;

        public EFStackExchangeCacheServiceProvider(
          IOptions<EFCoreSecondLevelCacheSettings> cacheSettings,
          IConfiguration Configuration,
          IRedisCacheClient redisCacheClient,
          IReaderWriterLockProvider readerWriterLockProvider,
          IMemoryCache memoryCache,
          IMemoryCacheChangeTokenProvider signal)
        {
            this._cacheSettings = cacheSettings?.Value;
            this._readerWriterLockProvider = readerWriterLockProvider;
            this._redisCacheClient = redisCacheClient;
            this._memoryCache = memoryCache;
            this._signal = signal;
            this._configuration = Configuration;
        }

        public void ClearAllCachedEntries()
        {
            if (this.IsConnectedRedis)
                this._readerWriterLockProvider.TryWriteLocked((Action)(() => this._redisCacheClient.Db0.FlushDbAsync()));
            this._readerWriterLockProvider.TryWriteLocked((Action)(() => this._signal.RemoveAllChangeTokens()));
        }

        public EFCachedData GetValue(EFCacheKey cacheKey, EFCachePolicy cachePolicy)
        {
            bool isConnectedRedis = this.IsConnectedRedis;
            return !this.IsConnectedRedis ? this._readerWriterLockProvider.TryReadLocked<EFCachedData>((Func<EFCachedData>)(() => this._memoryCache.Get<EFCachedData>((object)cacheKey.KeyHash))) : this._readerWriterLockProvider.TryReadLocked<EFCachedData>((Func<EFCachedData>)(() => this._redisCacheClient.Db0.GetAsync<EFCachedData>(cacheKey.KeyHash).Result));
        }

        public void InsertValue(EFCacheKey cacheKey, EFCachedData value, EFCachePolicy cachePolicy)
        {
            if (this.IsConnectedRedis)
                this._readerWriterLockProvider.TryWriteLocked((Action)(() =>
                {
                    if (value == null)
                        value = new EFCachedData() { IsNull = true };
                    string keyHash = cacheKey.KeyHash;
                    foreach (string cacheDependency in (IEnumerable<string>)cacheKey.CacheDependencies)
                    {
                        HashSet<string> result = this._redisCacheClient.Db0.GetAsync<HashSet<string>>(cacheDependency).Result;
                        if (result == null)
                        {
                            IRedisDatabase db0 = this._redisCacheClient.Db0;
                            string key = cacheDependency;
                            HashSet<string> stringSet = new HashSet<string>();
                            stringSet.Add(keyHash);
                            TimeSpan cacheTimeout = cachePolicy.CacheTimeout;
                            db0.AddAsync<HashSet<string>>(key, stringSet, cacheTimeout);
                        }
                        else
                        {
                            result.Add(keyHash);
                            this._redisCacheClient.Db0.AddAsync<HashSet<string>>(cacheDependency, result, cachePolicy.CacheTimeout);
                        }
                    }
                    this._redisCacheClient.Db0.AddAsync<EFCachedData>(keyHash, value, cachePolicy.CacheTimeout);
                }));
            else
                this._readerWriterLockProvider.TryWriteLocked((Action)(() =>
                {
                    if (value == null)
                        value = new EFCachedData() { IsNull = true };
                    MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
                    {
                        Size = new long?(1L)
                    };
                    if (cachePolicy.CacheExpirationMode == CacheExpirationMode.Absolute)
                        options.AbsoluteExpirationRelativeToNow = new TimeSpan?(cachePolicy.CacheTimeout);
                    else
                        options.SlidingExpiration = new TimeSpan?(cachePolicy.CacheTimeout);
                    foreach (string cacheDependency in (IEnumerable<string>)cacheKey.CacheDependencies)
                        options.ExpirationTokens.Add(this._signal.GetChangeToken(cacheDependency));
                    this._memoryCache.Set<EFCachedData>((object)cacheKey.KeyHash, value, options);
                }));
        }

        public void InvalidateCacheDependencies(EFCacheKey cacheKey)
        {
            if (this.IsConnectedRedis)
            {
                this._readerWriterLockProvider.TryWriteLocked((Action)(() =>
                {
                    foreach (string cacheDependency in (IEnumerable<string>)cacheKey.CacheDependencies)
                    {
                        if (!string.IsNullOrWhiteSpace(cacheDependency))
                            this.clearDependencyValues(cacheDependency);
                    }
                }));
            }
            else
            {
                foreach (string cacheDependency in (IEnumerable<string>)cacheKey.CacheDependencies)
                {
                    string rootCacheKey = cacheDependency;
                    this._readerWriterLockProvider.TryWriteLocked((Action)(() => this._signal.RemoveChangeToken(rootCacheKey)));
                }
            }
        }

        private async void clearDependencyValues(string rootCacheKey)
        {
            HashSet<string> dependencyKeys = await this._redisCacheClient.Db0.GetAsync<HashSet<string>>(rootCacheKey);
            if (dependencyKeys == null)
            {
                dependencyKeys = (HashSet<string>)null;
            }
            else
            {
                foreach (string dependencyKey in dependencyKeys)
                {
                    int num = await this._redisCacheClient.Db0.RemoveAsync(dependencyKey) ? 1 : 0;
                }
                dependencyKeys = (HashSet<string>)null;
            }
        }

        public async void Remove(EFCacheKey cacheKey)
        {
            if (this.IsConnectedRedis)
            {
                int num = await this._readerWriterLockProvider.TryReadLocked<Task<bool>>((Func<Task<bool>>)(() => this._redisCacheClient.Db0.RemoveAsync(cacheKey.KeyHash))) ? 1 : 0;
            }
            else
                this._memoryCache.Remove((object)cacheKey.KeyHash);
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
                    //var redisKey = RedisKey.op_Impilicit($"{(object)redisConfiguration.Hosts[0].Host}:{(object)redisConfiguration.Hosts[0].Port}");
                    return ((IDatabaseAsync)this._redisCacheClient.Db0.Database).IsConnected(default(RedisKey), (CommandFlags)0);
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
