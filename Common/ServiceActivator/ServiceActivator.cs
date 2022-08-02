using Microsoft.Extensions.DependencyInjection;
using System;

namespace San.CoreCommon.ServiceActivator
{
  public class ServiceActivator
  {
    internal static IServiceProvider _serviceProvider;

    public static void Configure(IServiceProvider serviceProvider) => San.CoreCommon.ServiceActivator.ServiceActivator._serviceProvider = serviceProvider;

    public static IServiceScope GetScope(IServiceProvider serviceProvider = null)
    {
      IServiceProvider provider = serviceProvider ?? San.CoreCommon.ServiceActivator.ServiceActivator._serviceProvider;
      return provider != null ? provider.GetRequiredService<IServiceScopeFactory>().CreateScope() : (IServiceScope) null;
    }
  }
}
