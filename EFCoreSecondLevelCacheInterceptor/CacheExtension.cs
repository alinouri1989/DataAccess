// Decompiled with JetBrains decompiler
// Type: EFCoreSecondLevelCacheInterceptor.CacheExtension
// Assembly: DataAccess, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6804894C-F989-4432-B8EA-6F3F70ACE424
// Assembly location: C:\Users\sa.nori\AppData\Local\Temp\Riqygac\68676b643f\lib\net5.0\DataAccess.dll

using EasyCaching.Core;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace EFCoreSecondLevelCacheInterceptor
{
  public static class CacheExtension
  {
    public static TItem GetFromCache<TItem>(this IMemoryCache cache, object key) => cache.Get<TItem>(key);

    public static TItem GetFromCache<TItem>(this IEasyCachingProvider cache, string key)
    {
      if (((IEasyCachingProviderBase) cache).Get<TItem>(key).HasValue)
        return ((IEasyCachingProviderBase) cache).Get<TItem>(key).Value;
      throw new Exception("cache is empty");
    }

    public static void SetToCache<TItem>(
      this IMemoryCache cache,
      object key,
      TItem value,
      MemoryCacheEntryOptions options)
    {
      cache.Set<TItem>(key, value, options);
    }

    public static void SetToCache<TItem>(
      this IEasyCachingProvider cache,
      TItem value,
      string key,
      TimeSpan timeout)
    {
      ((IEasyCachingProviderBase) cache).Set<TItem>(key, value, timeout);
    }

    public static bool TryGet<TItem>(this IMemoryCache cache, object key, out TItem value) => cache.TryGetValue<TItem>(key, out value);

    public static bool TryGet<TItem>(this IEasyCachingProvider cache, object key, out TItem value) => cache.TryGet<TItem>(key, out value);
  }
}
