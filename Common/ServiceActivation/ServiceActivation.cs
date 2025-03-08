using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.Activation;

public static class ServiceActivator
{
    private static IServiceProvider? _serviceProvider;

    /// <summary>  
    /// Configures the ServiceActivator with the application's service provider.  
    /// This method must be called during application startup.  
    /// </summary>  
    /// <param name="serviceCollection">The service collection to build the provider from.</param>  
    /// <exception cref="ArgumentNullException">Thrown if the serviceCollection is null.</exception>  
    /// <exception cref="InvalidOperationException">Thrown if Configure is called more than once.</exception>  
    public static void Configure(IServiceCollection serviceCollection)
    {
        if (_serviceProvider != null)
        {
            throw new InvalidOperationException("ServiceActivator.Configure() can only be called once.");
        }

        _serviceProvider = serviceCollection.BuildServiceProvider() ?? throw new ArgumentNullException(nameof(serviceCollection));
    }

    /// <summary>  
    /// Resolves a service of type T from the service provider.  
    /// </summary>  
    /// <typeparam name="T">The type of the service to resolve.</typeparam>  
    /// <returns>The resolved service.</returns>  
    /// <exception cref="InvalidOperationException">Thrown if the ServiceActivator is not configured.</exception>  
    public static T ResolveService<T>() where T : notnull
    {
        EnsureConfigured();
        return _serviceProvider!.GetRequiredService<T>();
    }

    /// <summary>  
    /// Resolves a service of the specified type from the service provider.  
    /// </summary>  
    /// <param name="serviceType">The type of the service to resolve.</param>  
    /// <returns>The resolved service.</returns>  
    /// <exception cref="InvalidOperationException">Thrown if the ServiceActivator is not configured.</exception>  
    public static object ResolveService(Type serviceType)
    {
        EnsureConfigured();
        return _serviceProvider!.GetRequiredService(serviceType);
    }

    /// <summary>  
    /// Creates a new service scope.  
    /// </summary>  
    /// <returns>A new service scope.</returns>  
    /// <exception cref="InvalidOperationException">Thrown if the ServiceActivator is not configured.</exception>  
    public static IServiceScope CreateScope()
    {
        EnsureConfigured();
        return _serviceProvider!.GetRequiredService<IServiceScopeFactory>().CreateScope();
    }

    /// <summary>  
    /// Resolves a service of type T within a new service scope.  The serviceProvider parameter is now obsolete.  
    /// </summary>  
    /// <typeparam name="T">The type of the service to resolve.</typeparam>  
    /// <param name="serviceProvider">This parameter is obsolete and will be removed in a future version.</param>  
    /// <returns>The resolved service.</returns>  
    /// <exception cref="InvalidOperationException">Thrown if the ServiceActivator is not configured.</exception>  
    [Obsolete("The serviceProvider parameter is obsolete and will be removed in a future version.  It is no longer needed.", false)]
    public static T GetService<T>(IServiceProvider? serviceProvider = null) where T : notnull
    {
        using var scope = CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }

    private static void EnsureConfigured()
    {
        if (_serviceProvider == null)
        {
            throw new InvalidOperationException("ServiceActivator has not been configured.  Call Configure() during application startup.");
        }
    }
}