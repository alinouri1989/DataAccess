using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repository.Cache
{
    public static class StaticCacheProvider
    {
        public static IServiceCollection AddStaticCache<TEntity>(
          this IServiceCollection services,
          StaticCacheSetting<TEntity> staticCacheSetting)
          where TEntity : class
        {
            services.configOptions<TEntity>(staticCacheSetting);
            return services;
        }

        public static IServiceCollection AddStaticCache<TEntity, TResult>(
          this IServiceCollection services,
          StaticCacheSetting<TEntity, TResult> staticCacheSetting)
          where TEntity : class
          where TResult : class
        {
            services.configOptions<TEntity, TResult>(staticCacheSetting);
            return services;
        }

        private static void configOptions<TEntity>(
          this IServiceCollection services,
          StaticCacheSetting<TEntity> cacheOptions)
          where TEntity : class
        {
            TimeSpan timeout = cacheOptions.timeout;
            CacheExpirationMode expirationMode = cacheOptions.expirationMode;
            IServiceProvider serviceProvider = (IServiceProvider)services.BuildServiceProvider();
            cacheOptions.TypeName = typeof(TEntity).Name;
            StatiCachInst.add(cacheOptions);
            if (cacheOptions.Schedule == null)
            {
                Type type = typeof(GenericRepository<,>).MakeGenericType(typeof(TEntity));
                object[] objArray =
                {
                    serviceProvider.GetService(typeof (DbContext))
                };
                object instance = Activator.CreateInstance(type, objArray);
                object[] parameters =
                {
                     cacheOptions.timeout,
                     cacheOptions.expirationMode,
                     cacheOptions.cacheSelector,
                     cacheOptions.cacheFilter
                };
                ((IEnumerable<TEntity>)type.GetMethod("Cache").Invoke(instance, parameters)).ToList<TEntity>();
            }
            else
            {
                services.AddSingleton<UpdateCaheJob<TEntity>>();
                services.AddSingleton<JobSchedule>(new JobSchedule(typeof(UpdateCaheJob<TEntity>), cacheOptions.Schedule));
            }
        }

        private static void configOptions<TEntity, TResult>(
          this IServiceCollection services,
          StaticCacheSetting<TEntity, TResult> cacheOptions)
          where TEntity : class
          where TResult : class
        {
            TimeSpan timeout = cacheOptions.timeout;
            CacheExpirationMode expirationMode = cacheOptions.expirationMode;
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            cacheOptions.TypeName = typeof(TEntity).Name;
            StatiCachInst.add(cacheOptions);
            if (cacheOptions.Schedule == null)
            {
                Type type = typeof(GenericRepository<,>).MakeGenericType(typeof(TEntity));
                object[] objArray =
                [
                     serviceProvider.GetService(typeof (DbContext))
                ];
                object instance = Activator.CreateInstance(type, objArray);
                object[] parameters =
                [
                      cacheOptions.timeout,
                      cacheOptions.expirationMode,
                      cacheOptions.cacheSelector,
                      cacheOptions.cacheFilter
                ];
                ((IEnumerable<TResult>)type.GetMethod("Cache").MakeGenericMethod(typeof(TResult)).Invoke(instance, parameters)).ToList<TResult>();
            }
            else
            {
                services.AddSingleton<UpdateCaheJob<TEntity>>();
                services.AddSingleton(new JobSchedule(typeof(UpdateCaheJob<TEntity>), cacheOptions.Schedule));
            }
        }
    }
}
