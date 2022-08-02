using EasyCaching.Core;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Repository.Cache
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
      this._readerWriterLockProvider = readerWriterLockProvider;
      this._logger = logger;
      this._serviceProvider = serviceProvider;
    }

    public void ClearAllCachedEntries()
    {
      if (this.Provider == CachProviderConfig.ProviderEnum.memoryCach)
        this.MemoryCacheClearAllCachedEntries();
      if (this.Provider == CachProviderConfig.ProviderEnum.easyCaching)
        this.EasyCacheClearAllCachedEntries();
      if (this.Provider != CachProviderConfig.ProviderEnum.disttibutedCaching)
        return;
      this.DistributedCacheClearAllCachedEntries();
    }

    public EFCachedData GetValue(EFCacheKey cacheKey, EFCachePolicy cachePolicy)
    {
      if (this.Provider == CachProviderConfig.ProviderEnum.memoryCach)
      {
        this._logger.LogInformation("Get Data From Memory Chache with key " + cacheKey.KeyHash);
        return this.MemoryCacheGetValue(cacheKey, cachePolicy);
      }
      if (this.Provider == CachProviderConfig.ProviderEnum.disttibutedCaching)
      {
        this._logger.LogInformation("Get Data From Distributed Chache with key " + cacheKey.KeyHash);
        return this.DistributedCacheGetValue(cacheKey, cachePolicy);
      }
      if (this.Provider != CachProviderConfig.ProviderEnum.easyCaching)
        throw new Exception("cache provider not set");
      this._logger.LogInformation("Get Data From Easy Chache with key " + cacheKey.KeyHash);
      return this.EasyCacheGetValue(cacheKey, cachePolicy);
    }

    public void InsertValue(EFCacheKey cacheKey, EFCachedData value, EFCachePolicy cachePolicy)
    {
      if (this.Provider == CachProviderConfig.ProviderEnum.memoryCach)
      {
        this._logger.LogInformation("Set Data To memoryCach with key " + cacheKey.KeyHash);
        this.MemoryCacheInsertValue(cacheKey, value, cachePolicy);
      }
      else if (this.Provider == CachProviderConfig.ProviderEnum.disttibutedCaching)
      {
        this._logger.LogInformation("Set Data To disttibutedCaching with key " + cacheKey.KeyHash);
        this.DistributedCacheInsertValue(cacheKey, value, cachePolicy);
      }
      else
      {
        if (this.Provider != CachProviderConfig.ProviderEnum.easyCaching)
          throw new Exception("cach provider not set");
        this._logger.LogInformation("Set Data To easyCaching with key " + cacheKey.KeyHash);
        this.EasyCacheInsertValue(cacheKey, value, cachePolicy);
      }
    }

    public void InvalidateCacheDependencies(EFCacheKey cacheKey)
    {
      if (this.Provider == CachProviderConfig.ProviderEnum.memoryCach)
        this.MemoryCacheInvalidateCacheDependencies(cacheKey);
      if (this.Provider != CachProviderConfig.ProviderEnum.easyCaching)
        return;
      this.EasyCacheInvalidateCacheDependencies(cacheKey);
    }

    public void Remove(EFCacheKey cacheKey)
    {
      if (this.Provider == CachProviderConfig.ProviderEnum.memoryCach)
      {
        this.RemoveMemoryCache(cacheKey);
      }
      else
      {
        if (this.Provider == CachProviderConfig.ProviderEnum.disttibutedCaching || this.Provider != CachProviderConfig.ProviderEnum.easyCaching)
          return;
        this.RemoveEasyCache(cacheKey);
      }
    }

    private EFCachedData MemoryCacheGetValue(
      EFCacheKey cacheKey,
      EFCachePolicy cachePolicy)
    {
      IMemoryCache _memoryCache = this._serviceProvider.GetRequiredService<IMemoryCache>();
      return this._readerWriterLockProvider.TryReadLocked<EFCachedData>((Func<EFCachedData>) (() => _memoryCache.Get<EFCachedData>((object) cacheKey.KeyHash)));
    }

    private void MemoryCacheInsertValue(
      EFCacheKey cacheKey,
      EFCachedData value,
      EFCachePolicy cachePolicy)
    {
      IMemoryCacheChangeTokenProvider _signal = this._serviceProvider.GetRequiredService<IMemoryCacheChangeTokenProvider>();
      IMemoryCache _memoryCache = this._serviceProvider.GetRequiredService<IMemoryCache>();
      this._readerWriterLockProvider.TryWriteLocked((Action) (() =>
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
        foreach (string cacheDependency in (IEnumerable<string>) cacheKey.CacheDependencies)
          options.ExpirationTokens.Add(_signal.GetChangeToken(cacheDependency));
        _memoryCache.Set<EFCachedData>((object) cacheKey.KeyHash, value, options);
      }));
    }

    private void MemoryCacheClearAllCachedEntries()
    {
      IMemoryCacheChangeTokenProvider _signal = this._serviceProvider.GetRequiredService<IMemoryCacheChangeTokenProvider>();
      this._readerWriterLockProvider.TryWriteLocked((Action) (() => _signal.RemoveAllChangeTokens()));
    }

    private void MemoryCacheInvalidateCacheDependencies(EFCacheKey cacheKey)
    {
      IMemoryCacheChangeTokenProvider _signal = this._serviceProvider.GetRequiredService<IMemoryCacheChangeTokenProvider>();
      foreach (string cacheDependency in (IEnumerable<string>) cacheKey.CacheDependencies)
      {
        string rootCacheKey = cacheDependency;
        this._readerWriterLockProvider.TryWriteLocked((Action) (() => _signal.RemoveChangeToken(rootCacheKey)));
      }
    }

    private void RemoveMemoryCache(EFCacheKey cacheKey) => this._serviceProvider.GetRequiredService<IMemoryCache>().Remove((object) cacheKey.KeyHash);

    private EFCachedData EasyCacheGetValue(
      EFCacheKey cacheKey,
      EFCachePolicy cachePolicy)
    {
      IEasyCachingProvider _easyCachingProvider = this._serviceProvider.GetRequiredService<IEasyCachingProvider>();
      return this._readerWriterLockProvider.TryReadLocked<EFCachedData>((Func<EFCachedData>) (() => ((IEasyCachingProviderBase) _easyCachingProvider).Get<EFCachedData>(cacheKey.KeyHash).Value));
    }

    private void EasyCacheInsertValue(
      EFCacheKey cacheKey,
      EFCachedData value,
      EFCachePolicy cachePolicy)
    {
      IEasyCachingProvider _easyCachingProvider = this._serviceProvider.GetRequiredService<IEasyCachingProvider>();
      this._readerWriterLockProvider.TryWriteLocked((Action) (() =>
      {
        if (value == null)
          value = new EFCachedData() { IsNull = true };
        string keyHash = cacheKey.KeyHash;
        foreach (string cacheDependency in (IEnumerable<string>) cacheKey.CacheDependencies)
        {
          CacheValue<HashSet<string>> cacheValue = ((IEasyCachingProviderBase) _easyCachingProvider).Get<HashSet<string>>(cacheDependency);
          if (cacheValue.IsNull)
          {
            IEasyCachingProvider ieasyCachingProvider = _easyCachingProvider;
            string str = cacheDependency;
            HashSet<string> stringSet = new HashSet<string>();
            stringSet.Add(keyHash);
            TimeSpan cacheTimeout = cachePolicy.CacheTimeout;
            ((IEasyCachingProviderBase) ieasyCachingProvider).Set<HashSet<string>>(str, stringSet, cacheTimeout);
          }
          else
          {
            cacheValue.Value.Add(keyHash);
            ((IEasyCachingProviderBase) _easyCachingProvider).Set<HashSet<string>>(cacheDependency, cacheValue.Value, cachePolicy.CacheTimeout);
          }
        }
        ((IEasyCachingProviderBase) _easyCachingProvider).Set<EFCachedData>(keyHash, value, cachePolicy.CacheTimeout);
      }));
    }

    private void EasyCacheClearAllCachedEntries()
    {
      IEasyCachingProvider _easyCachingProvider = this._serviceProvider.GetRequiredService<IEasyCachingProvider>();
      this._readerWriterLockProvider.TryWriteLocked((Action) (() => _easyCachingProvider.Flush()));
    }

    private void EasyCacheInvalidateCacheDependencies(EFCacheKey cacheKey)
    {
      IEasyCachingProvider _easyCachingProvider = this._serviceProvider.GetRequiredService<IEasyCachingProvider>();
      this._readerWriterLockProvider.TryWriteLocked((Action) (() =>
      {
        foreach (string cacheDependency in (IEnumerable<string>) cacheKey.CacheDependencies)
        {
          if (!string.IsNullOrWhiteSpace(cacheDependency))
          {
            this.clearDependencyValues(cacheDependency);
            ((IEasyCachingProviderBase) _easyCachingProvider).Remove(cacheDependency);
          }
        }
      }));
    }

    private void clearDependencyValues(string rootCacheKey)
    {
      IEasyCachingProvider requiredService = this._serviceProvider.GetRequiredService<IEasyCachingProvider>();
      CacheValue<HashSet<string>> cacheValue = ((IEasyCachingProviderBase) requiredService).Get<HashSet<string>>(rootCacheKey);
      if (cacheValue.IsNull)
        return;
      foreach (string str in cacheValue.Value)
        ((IEasyCachingProviderBase) requiredService).Remove(str);
    }

    private void RemoveEasyCache(EFCacheKey cacheKey)
    {
      IEasyCachingProvider _easyCachingProvider = this._serviceProvider.GetRequiredService<IEasyCachingProvider>();
      this._readerWriterLockProvider.TryReadLocked((Action) (() => ((IEasyCachingProviderBase) _easyCachingProvider).Remove(cacheKey.KeyHash)));
    }

    private EFCachedData DistributedCacheGetValue(
      EFCacheKey cacheKey,
      EFCachePolicy cachePolicy)
    {
      IDistributedCache _distributedCache = this._serviceProvider.GetRequiredService<IDistributedCache>();
      return this._readerWriterLockProvider.TryReadLocked<EFCachedData>((Func<EFCachedData>) (() => _distributedCache.Get(cacheKey.KeyHash).FromByteArray<EFCachedData>()));
    }

    private void DistributedCacheInsertValue(
      EFCacheKey cacheKey,
      EFCachedData value,
      EFCachePolicy cachePolicy)
    {
      IDistributedCache _distributedCache = this._serviceProvider.GetRequiredService<IDistributedCache>();
      this._readerWriterLockProvider.TryWriteLocked((Action) (() =>
      {
        if (value == null)
          value = new EFCachedData() { IsNull = true };
        string keyHash = cacheKey.KeyHash;
        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
        {
          AbsoluteExpirationRelativeToNow = new TimeSpan?(cachePolicy.CacheTimeout)
        };
        foreach (string cacheDependency in (IEnumerable<string>) cacheKey.CacheDependencies)
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
      IDistributedCache _distributedCache = this._serviceProvider.GetRequiredService<IDistributedCache>();
      this._readerWriterLockProvider.TryReadLocked((Action) (() => _distributedCache.Remove(cacheKey.KeyHash)));
    }
  }
}
