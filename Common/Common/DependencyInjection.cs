using Common.Export;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
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

        public static void RegisterExport(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICsvService, CsvService>();
            builder.Services.AddScoped<IExcelService, ExcelService>();
            builder.Services.AddScoped<IHtmlService, HtmlService>();
            builder.Services.AddScoped<IJsonService, JsonService>();
            builder.Services.AddScoped<IXmlService, XmlService>();
            builder.Services.AddScoped<IYamlService, YamlService>();
            builder.Services.AddScoped(typeof(IExportService<>), typeof(ExportService<>));
        }

        public static IServiceCollection RegisterCommon(
          this IServiceCollection services)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return ServiceCollectionExtensions.Scan(services,
                scan =>
                scan.FromAssemblies(assemblies).
                AddClasses(c => c.WithAttribute<SingletonServiceAttribute>())
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
