using EasyCaching.Core;
using DataAccess.EFCoreSecondLevelCacheInterceptor;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DataAccess.Repository.Cache
{
    public class CachProvider : CachProviderConfig, IEFCacheServiceProvider
    {
        private readonly ILogger<CachProvider> _logger;
        private readonly IReaderWriterLockProvider _readerWriterLockProvider;
        private readonly IServiceProvider _serviceProvider;

        public CachProvider(
          ILogger<CachProvider> logger,
          IConfiguration configuration,
          IServiceProvider serviceProvider,
          IReaderWriterLockProvider readerWriterLockProvider)
          : base(configuration)
        {
            _readerWriterLockProvider = readerWriterLockProvider;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public void ClearAllCachedEntries()
        {
            if (Provider == ProviderEnum.memoryCach)
                MemoryCacheClearAllCachedEntries();
            if (Provider == ProviderEnum.easyCaching)
                EasyCacheClearAllCachedEntries();
            if (Provider != ProviderEnum.disttibutedCaching)
                return;
            DistributedCacheClearAllCachedEntries();
        }

        public EFCachedData GetValue(EFCacheKey cacheKey, EFCachePolicy cachePolicy)
        {
            if (Provider == ProviderEnum.memoryCach)
            {
                _logger.LogInformation("Get Data From Memory Chache with key " + cacheKey.KeyHash);
                return MemoryCacheGetValue(cacheKey, cachePolicy);
            }
            if (Provider == ProviderEnum.disttibutedCaching)
            {
                _logger.LogInformation("Get Data From Distributed Chache with key " + cacheKey.KeyHash);
                return DistributedCacheGetValue(cacheKey, cachePolicy);
            }
            if (Provider != ProviderEnum.easyCaching)
                throw new Exception("cache provider not set");
            _logger.LogInformation("Get Data From Easy Chache with key " + cacheKey.KeyHash);
            return EasyCacheGetValue(cacheKey, cachePolicy);
        }

        public void InsertValue(EFCacheKey cacheKey, EFCachedData value, EFCachePolicy cachePolicy)
        {
            if (Provider == ProviderEnum.memoryCach)
            {
                _logger.LogInformation("Set Data To memoryCach with key " + cacheKey.KeyHash);
                MemoryCacheInsertValue(cacheKey, value, cachePolicy);
            }
            else if (Provider == ProviderEnum.disttibutedCaching)
            {
                _logger.LogInformation("Set Data To disttibutedCaching with key " + cacheKey.KeyHash);
                DistributedCacheInsertValue(cacheKey, value, cachePolicy);
            }
            else
            {
                if (Provider != ProviderEnum.easyCaching)
                    throw new Exception("cach provider not set");
                _logger.LogInformation("Set Data To easyCaching with key " + cacheKey.KeyHash);
                EasyCacheInsertValue(cacheKey, value, cachePolicy);
            }
        }

        public void InvalidateCacheDependencies(EFCacheKey cacheKey)
        {
            if (Provider == ProviderEnum.memoryCach)
                MemoryCacheInvalidateCacheDependencies(cacheKey);
            if (Provider != ProviderEnum.easyCaching)
                return;
            EasyCacheInvalidateCacheDependencies(cacheKey);
        }

        public void Remove(EFCacheKey cacheKey)
        {
            if (Provider == ProviderEnum.memoryCach)
            {
                RemoveMemoryCache(cacheKey);
            }
            else
            {
                if (Provider == ProviderEnum.disttibutedCaching || Provider != ProviderEnum.easyCaching)
                    return;
                RemoveEasyCache(cacheKey);
            }
        }

        private EFCachedData MemoryCacheGetValue(
          EFCacheKey cacheKey,
          EFCachePolicy cachePolicy)
        {
            IMemoryCache _memoryCache = _serviceProvider.GetRequiredService<IMemoryCache>();
            return _readerWriterLockProvider.TryReadLocked<EFCachedData>((Func<EFCachedData>)(() => _memoryCache.Get<EFCachedData>((object)cacheKey.KeyHash)));
        }

        private void MemoryCacheInsertValue(
          EFCacheKey cacheKey,
          EFCachedData value,
          EFCachePolicy cachePolicy)
        {
            IMemoryCacheChangeTokenProvider _signal = _serviceProvider.GetRequiredService<IMemoryCacheChangeTokenProvider>();
            IMemoryCache _memoryCache = _serviceProvider.GetRequiredService<IMemoryCache>();
            _readerWriterLockProvider.TryWriteLocked((Action)(() =>
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
                    options.ExpirationTokens.Add(_signal.GetChangeToken(cacheDependency));
                _memoryCache.Set<EFCachedData>((object)cacheKey.KeyHash, value, options);
            }));
        }

        private void MemoryCacheClearAllCachedEntries()
        {
            IMemoryCacheChangeTokenProvider _signal = _serviceProvider.GetRequiredService<IMemoryCacheChangeTokenProvider>();
            _readerWriterLockProvider.TryWriteLocked((Action)(() => _signal.RemoveAllChangeTokens()));
        }

        private void MemoryCacheInvalidateCacheDependencies(EFCacheKey cacheKey)
        {
            IMemoryCacheChangeTokenProvider _signal = _serviceProvider.GetRequiredService<IMemoryCacheChangeTokenProvider>();
            foreach (string cacheDependency in (IEnumerable<string>)cacheKey.CacheDependencies)
            {
                string rootCacheKey = cacheDependency;
                _readerWriterLockProvider.TryWriteLocked((Action)(() => _signal.RemoveChangeToken(rootCacheKey)));
            }
        }

        private void RemoveMemoryCache(EFCacheKey cacheKey) => _serviceProvider.GetRequiredService<IMemoryCache>().Remove((object)cacheKey.KeyHash);

        private EFCachedData EasyCacheGetValue(
          EFCacheKey cacheKey,
          EFCachePolicy cachePolicy)
        {
            IEasyCachingProvider _easyCachingProvider = _serviceProvider.GetRequiredService<IEasyCachingProvider>();
            return _readerWriterLockProvider.TryReadLocked<EFCachedData>((Func<EFCachedData>)(() => ((IEasyCachingProviderBase)_easyCachingProvider).Get<EFCachedData>(cacheKey.KeyHash).Value));
        }

        private void EasyCacheInsertValue(
          EFCacheKey cacheKey,
          EFCachedData value,
          EFCachePolicy cachePolicy)
        {
            IEasyCachingProvider _easyCachingProvider = _serviceProvider.GetRequiredService<IEasyCachingProvider>();
            _readerWriterLockProvider.TryWriteLocked((Action)(() =>
            {
                if (value == null)
                    value = new EFCachedData() { IsNull = true };
                string keyHash = cacheKey.KeyHash;
                foreach (string cacheDependency in (IEnumerable<string>)cacheKey.CacheDependencies)
                {
                    CacheValue<HashSet<string>> cacheValue = ((IEasyCachingProviderBase)_easyCachingProvider).Get<HashSet<string>>(cacheDependency);
                    if (cacheValue.IsNull)
                    {
                        IEasyCachingProvider ieasyCachingProvider = _easyCachingProvider;
                        string str = cacheDependency;
                        HashSet<string> stringSet = new HashSet<string>();
                        stringSet.Add(keyHash);
                        TimeSpan cacheTimeout = cachePolicy.CacheTimeout;
                        ((IEasyCachingProviderBase)ieasyCachingProvider).Set<HashSet<string>>(str, stringSet, cacheTimeout);
                    }
                    else
                    {
                        cacheValue.Value.Add(keyHash);
                        ((IEasyCachingProviderBase)_easyCachingProvider).Set<HashSet<string>>(cacheDependency, cacheValue.Value, cachePolicy.CacheTimeout);
                    }
                }
              ((IEasyCachingProviderBase)_easyCachingProvider).Set<EFCachedData>(keyHash, value, cachePolicy.CacheTimeout);
            }));
        }

        private void EasyCacheClearAllCachedEntries()
        {
            IEasyCachingProvider _easyCachingProvider = _serviceProvider.GetRequiredService<IEasyCachingProvider>();
            _readerWriterLockProvider.TryWriteLocked((Action)(() => _easyCachingProvider.Flush()));
        }

        private void EasyCacheInvalidateCacheDependencies(EFCacheKey cacheKey)
        {
            IEasyCachingProvider _easyCachingProvider = _serviceProvider.GetRequiredService<IEasyCachingProvider>();
            _readerWriterLockProvider.TryWriteLocked((Action)(() =>
            {
                foreach (string cacheDependency in (IEnumerable<string>)cacheKey.CacheDependencies)
                {
                    if (!string.IsNullOrWhiteSpace(cacheDependency))
                    {
                        clearDependencyValues(cacheDependency);
                        ((IEasyCachingProviderBase)_easyCachingProvider).Remove(cacheDependency);
                    }
                }
            }));
        }

        private void clearDependencyValues(string rootCacheKey)
        {
            IEasyCachingProvider requiredService = _serviceProvider.GetRequiredService<IEasyCachingProvider>();
            CacheValue<HashSet<string>> cacheValue = ((IEasyCachingProviderBase)requiredService).Get<HashSet<string>>(rootCacheKey);
            if (cacheValue.IsNull)
                return;
            foreach (string str in cacheValue.Value)
                ((IEasyCachingProviderBase)requiredService).Remove(str);
        }

        private void RemoveEasyCache(EFCacheKey cacheKey)
        {
            IEasyCachingProvider _easyCachingProvider = _serviceProvider.GetRequiredService<IEasyCachingProvider>();
            _readerWriterLockProvider.TryReadLocked((Action)(() => ((IEasyCachingProviderBase)_easyCachingProvider).Remove(cacheKey.KeyHash)));
        }

        private EFCachedData DistributedCacheGetValue(
          EFCacheKey cacheKey,
          EFCachePolicy cachePolicy)
        {
            IDistributedCache _distributedCache = _serviceProvider.GetRequiredService<IDistributedCache>();
            return _readerWriterLockProvider.TryReadLocked<EFCachedData>((Func<EFCachedData>)(() => _distributedCache.Get(cacheKey.KeyHash).FromByteArray<EFCachedData>()));
        }

        private void DistributedCacheInsertValue(
          EFCacheKey cacheKey,
          EFCachedData value,
          EFCachePolicy cachePolicy)
        {
            IDistributedCache _distributedCache = _serviceProvider.GetRequiredService<IDistributedCache>();
            _readerWriterLockProvider.TryWriteLocked((Action)(() =>
            {
                if (value == null)
                    value = new EFCachedData() { IsNull = true };
                string keyHash = cacheKey.KeyHash;
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = new TimeSpan?(cachePolicy.CacheTimeout)
                };
                foreach (string cacheDependency in (IEnumerable<string>)cacheKey.CacheDependencies)
                {
                    CacheValue<HashSet<string>> cacheValue = _distributedCache.Get(cacheDependency).FromByteArray<CacheValue<HashSet<string>>>();
                    if (cacheValue.IsNull)
                    {
                        _distributedCache.Set(cacheDependency, keyHash.ToByteArray(), options);
                    }
                    else
                    {
                        cacheValue.Value.Add(keyHash);
                        _distributedCache.Set(cacheDependency, cacheValue.Value.ToByteArray(), options);
                    }
                }
                _distributedCache.Set(keyHash, value.ToByteArray(), options);
            }));
        }

        private void DistributedCacheClearAllCachedEntries()
        {
        }

        private void RemoveDistributedCache(EFCacheKey cacheKey)
        {
            IDistributedCache _distributedCache = _serviceProvider.GetRequiredService<IDistributedCache>();
            _readerWriterLockProvider.TryReadLocked((Action)(() => _distributedCache.Remove(cacheKey.KeyHash)));
        }
    }
}
