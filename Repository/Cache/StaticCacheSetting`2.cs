// Decompiled with JetBrains decompiler
// Type: Repository.Cache.StaticCacheSetting`2
// Assembly: DataAccess, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6804894C-F989-4432-B8EA-6F3F70ACE424
// Assembly location: C:\Users\sa.nori\AppData\Local\Temp\Riqygac\68676b643f\lib\net5.0\DataAccess.dll

using EFCoreSecondLevelCacheInterceptor;
using Quartz;
using System;
using System.Linq.Expressions;

namespace Repository.Cache
{
  public class StaticCacheSetting<T, TResult>
  {
    public CacheExpirationMode expirationMode { get; set; }

    public TimeSpan timeout { get; set; }

    public Expression<Func<T, TResult>> cacheSelector { get; set; }

    public Expression<Func<T, bool>> cacheFilter { get; set; }

    public Action<SimpleScheduleBuilder> Schedule { get; set; }

    public string TypeName { get; set; }
  }
}
