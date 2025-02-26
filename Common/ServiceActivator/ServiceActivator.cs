using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.Activation
{
    public static class ServiceActivator
    {
        private static IServiceProvider _serviceProvider;

        // Configure the static service provider  
        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        // Resolve a service directly from the service provider  
        public static T ResolveService<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        // Optionally, for resolving services without a specific type  
        public static object ResolveService(Type serviceType)
        {
            return _serviceProvider.GetRequiredService(serviceType);
        }

        // Create a Scope from the existing Service Provider  
        public static IServiceScope CreateScope()
        {
            // Return a new scope from the existing service provider without creating a new instance  
            return _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        }

        // Resolve a service within a new scope  
        public static T GetService<T>(IServiceProvider serviceProvider = null)
        {
            using var scope = CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }
    }
}
