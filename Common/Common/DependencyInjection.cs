using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Common
{
    public static class DependencyInjection
    {
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
