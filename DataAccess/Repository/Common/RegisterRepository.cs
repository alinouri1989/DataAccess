using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Base;

namespace DataAccess.Repository.Common
{
    public static class RegisterRepository
    {
        public static IServiceCollection RegisterRepositoryBase<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
        {
            services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IBaseEf, BaseEf<TContext>>();
            return services;
        }
    }
}
