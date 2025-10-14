using Common.Caching.Models;
using Common.Export;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Caching
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceIfNotExist<TService>(this IServiceCollection services, string serviceName, Func<IServiceProvider, TService> implementationFactory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TService : class
        {
            bool alreadyRegistered = services.Any(
                sd => sd.ServiceType == typeof(TService) && sd.ImplementationFactory != null &&
                      sd.ImplementationFactory.Target.GetType().FullName == serviceName);

            if (!alreadyRegistered)
            {
                services.Add(new ServiceDescriptor(
                    typeof(TService),
                    implementationFactory,
                    lifetime));
            }

            return services;
        }

        private static IServiceCollection AddMemoryCacheIfNotExist(this IServiceCollection services)
        {
            if (!services.Any(sd => sd.ServiceType == typeof(IMemoryCache)))
            {
                services.AddMemoryCache();
            }
            return services;
        }

        public static void RegisterCache(this IServiceCollection services, IConfiguration configuration)
        {
            // ثبت تنظیمات Redis
            services.Configure<RedisConfiguration>(configuration.GetSection("Redis"));

            // ثبت تنظیمات Cache
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));

            var rdc = configuration.GetSection("Redis").Get<RedisConfiguration>();
            services.AddSingleton<IConnectionMultiplexer>(option =>
                           ConnectionMultiplexer.Connect(new ConfigurationOptions
                           {
                               EndPoints = { $"{rdc.Hosts.FirstOrDefault().Host}:{rdc.Hosts.FirstOrDefault().Port}" },
                               AbortOnConnectFail = rdc.AbortOnConnectFail,
                               Ssl = rdc.Ssl,
                               Password = rdc.Password,
                               ConnectRetry = rdc.ConnectRetry.HasValue ? rdc.ConnectRetry.Value : 2,
                               SyncTimeout = rdc.SyncTimeout
                           }));

            services.AddMemoryCacheIfNotExist();
            services.AddSingleton<ICacheService, CacheService>();
        }

        public static IServiceCollection RegisterWithAttribute(
          this IServiceCollection services)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return ServiceCollectionExtensions.Scan(services,
                scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(c => c.WithAttribute<SingletonServiceAttribute>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
                .AddClasses(c => c.WithAttribute<TransientServiceAttribute>())
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(c => c.WithAttribute<ScopedServiceAttribute>())
                .AsMatchingInterface()
                .WithScopedLifetime()
                );
        }
    }
}
