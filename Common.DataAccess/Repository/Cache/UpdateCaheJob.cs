﻿using Common.DataAccess.EFCoreSecondLevelCacheInterceptor;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.DataAccess.Repository.Cache
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
            this._logger.LogInformation("Cache job start for" + typeof(T).Name);
            Expression<Func<T, T>> expression1 = null;
            Expression<Func<T, bool>> expression2 = null;
            TimeSpan timeSpan = new TimeSpan();
            CacheExpirationMode cacheExpirationMode = CacheExpirationMode.Absolute;
            foreach (object obj in StatiCachInst.Get())
            {
                if (obj.GetType().GetGenericArguments().FirstOrDefault().Equals(typeof(T)))
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
