﻿// Decompiled with JetBrains decompiler
// Type: Repository.Cache.UpdateCaheJob`1
// Assembly: DataAccess, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6804894C-F989-4432-B8EA-6F3F70ACE424
// Assembly location: C:\Users\sa.nori\AppData\Local\Temp\Riqygac\68676b643f\lib\net5.0\DataAccess.dll

using EFCoreSecondLevelCacheInterceptor;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.Cache
{
  public class UpdateCaheJob<T> : IJob where T : class
  {
    private readonly ILogger<UpdateCaheJob<T>> _logger;
    private readonly IServiceProvider _serviceProvider;

    public UpdateCaheJob(ILogger<UpdateCaheJob<T>> logger, IServiceProvider serviceProvider)
    {
      this._logger = logger;
      this._serviceProvider = serviceProvider;
    }

    public Task Execute(IJobExecutionContext context)
    {
      this._logger.LogInformation("Cache job start for" + typeof (T).Name);
      Expression<Func<T, T>> expression1 = (Expression<Func<T, T>>) null;
      Expression<Func<T, bool>> expression2 = (Expression<Func<T, bool>>) null;
      TimeSpan timeSpan = new TimeSpan();
      CacheExpirationMode cacheExpirationMode = CacheExpirationMode.Absolute;
      foreach (object obj in StatiCachInst.Get())
      {
        if (((IEnumerable<Type>) obj.GetType().GetGenericArguments()).FirstOrDefault<Type>().Equals(typeof (T)))
        {
          StaticCacheSetting<T> staticCacheSetting = obj as StaticCacheSetting<T>;
          expression1 = staticCacheSetting.cacheSelector;
          expression2 = staticCacheSetting.cacheFilter;
          timeSpan = staticCacheSetting.timeout;
          cacheExpirationMode = staticCacheSetting.expirationMode;
        }
      }
      return Task.CompletedTask;
    }
  }
}
