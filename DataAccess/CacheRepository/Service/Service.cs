using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Repository.Service
{
    public abstract class Service<T, TContext> where T : class
    {
        private readonly IServiceProvider _serviceProvider;

        public Service(IServiceProvider serviceProvider) => this._serviceProvider = serviceProvider;

        public IGenericRepository<T, TContext> reopsitory => this.GetRepository<T, TContext>();

        public abstract IQueryable<T> Get(
          Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
          params Expression<Func<T, object>>[] includeProperties);

        public IGenericRepository<A, AContext> GetRepository<A, AContext>() where A : class => this._serviceProvider.GetRequiredService<IGenericRepository<A, AContext>>();

        public abstract T GetById(object Id);
    }
}
