using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using San.CoreCommon.Attribute;
using Scrutor;
using System;
using System.Reflection;

namespace Common
{
  public static class DependencyInjection
  {
    public static IServiceCollection RegisterCommon(
      this IServiceCollection services,
      IConfiguration configuration)
    {
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      return ServiceCollectionExtensions.Scan(services, (Action<ITypeSourceSelector>) (scan => ((IAssemblySelector) scan).FromAssemblies(assemblies).AddClasses((Action<IImplementationTypeFilter>) (c => c.WithAttribute<SingletonServiceAttribute>())).AsImplementedInterfaces().WithSingletonLifetime().AddClasses((Action<IImplementationTypeFilter>) (c => c.WithAttribute<TransientServiceAttribute>())).AsImplementedInterfaces().WithTransientLifetime().AddClasses((Action<IImplementationTypeFilter>) (c => c.WithAttribute<ScopedServiceAttribute>())).AsMatchingInterface().WithScopedLifetime()));
    }
  }
}
