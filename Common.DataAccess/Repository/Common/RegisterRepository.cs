using Common.DataAccess.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.DataAccess.Repository.Common
{
    public static class RegisterRepository
    {
        public static IServiceCollection RegisterRepositoryBase<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            return services;
        }
    }
}
