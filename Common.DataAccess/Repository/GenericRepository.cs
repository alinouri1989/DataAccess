using Common.DataAccess.EFCoreSecondLevelCacheInterceptor;
using Common.DataAccess.Repository.Cache;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace Common.DataAccess.Repository
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<
#nullable disable
    TEntity, TContext>
      where TEntity : class
      where TContext : DbContext
    {
        private readonly DbSet<TEntity> dbSet;

        public TContext Context { get; }

        public GenericRepository(TContext context)
        {
            this.Context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Get(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> source = dbSet;
            if (filter != null)
                source = source.Where(filter);
            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
                source = source.Include(includeProperty);
            return orderBy != null ? orderBy(source) : source;
        }

        public virtual IQueryable<TType> Cache<TType>(
          TimeSpan timeout,
          CacheExpirationMode cacheExpirationMode,
          Expression<Func<TEntity, TType>> selector = null,
          Expression<Func<TEntity, bool>> filter = null)
          where TType : class
        {
            IQueryable<TEntity> queryable = dbSet;
            if (filter != null)
                queryable = queryable.Where(filter);
            return queryable.Cacheable<TEntity>(cacheExpirationMode, timeout).Select(selector);
        }

        public virtual List<TEntity> GetFromStaticCache()
        {
            IQueryable<TEntity> source = dbSet;
            Expression<Func<TEntity, TEntity>> selector = null;
            Expression<Func<TEntity, bool>> predicate = null;
            foreach (object obj in StatiCachInst.Get())
            {
                if (obj.GetType().GetGenericArguments().FirstOrDefault().Equals(typeof(TEntity)))
                {
                    StaticCacheSetting<TEntity> staticCacheSetting = obj as StaticCacheSetting<TEntity>;
                    selector = staticCacheSetting.cacheSelector;
                    predicate = staticCacheSetting.cacheFilter;
                }
            }
            if (predicate != null)
                source = source.Where(predicate);
            if (selector != null)
                source = source.Select(selector);
            return source.ToList();
        }

        public virtual TEntity GetByID(object id) => this.dbSet.Find(id);

        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            TEntity async = await this.dbSet.FindAsync(id);
            return async;
        }

        public virtual IQueryable<TEntity> GetAll(bool Cacheable = false) => Cacheable ? this.dbSet.Cacheable<TEntity>() : (IQueryable<TEntity>)this.dbSet;

        public virtual Task<TEntity> FirstOrDefaultAsync(
          Expression<Func<TEntity, bool>> filter = null,
          params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> source = dbSet;
            if (filter != null)
                source = source.Where(filter);
            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
                source = source.Include(includeProperty);
            return source.FirstOrDefaultAsync();
        }

        public virtual void Insert(TEntity entity) => this.dbSet.Add(entity);

        public virtual void Update(TEntity entity, string[] properities)
        {
            this.dbSet.Attach(entity);
            EntityEntry<TEntity> entityEntry = this.Context.Entry(entity);
            entityEntry.State = EntityState.Modified;
            foreach (string properity in properities)
            {
                this.Context.Entry(entity).Property(properity);
                entityEntry.Property(properity).IsModified = false;
            }
        }

        public virtual void Update(
          TEntity entityToUpdate,
          params Expression<Func<TEntity, object>>[] excludePropertyExpression)
        {
            this.dbSet.Attach(entityToUpdate);
            this.Context.Entry(entityToUpdate).State = EntityState.Modified;
            if (excludePropertyExpression == null)
                return;
            foreach (Expression<Func<TEntity, object>> propertyExpression in excludePropertyExpression)
                this.Context.Entry(entityToUpdate).Property(propertyExpression).IsModified = false;
        }

        public virtual void UpdateField(
          TEntity entityToUpdate,
          params Expression<Func<TEntity, object>>[] includePropertyExpression)
        {
            this.dbSet.Attach(entityToUpdate);
            if (includePropertyExpression == null)
                return;
            foreach (Expression<Func<TEntity, object>> propertyExpression in includePropertyExpression)
                this.Context.Entry(entityToUpdate).Property(propertyExpression).IsModified = true;
        }

        public virtual void AddRange(IEnumerable<TEntity> entities) => this.dbSet.AddRange(entities);

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) => await this.dbSet.AddRangeAsync(entities);

        public virtual void RemoveRange(IEnumerable<TEntity> entities) => this.dbSet.RemoveRange(entities);

        public virtual void Remove(TEntity entitiy) => this.dbSet.Remove(entitiy);

        public IQueryable<TEntity> ExecuteReader(string sqlQuery, params object[] parametrs)
        {
            try
            {
                return dbSet.FromSqlRaw(sqlQuery, parametrs);
            }
            catch (Exception ex)
            {
                throw new Exception("error on ExecuteReader on generic repository", ex);
            }
        }

        public void ExecuteNonQuery(string commandText, SqlParameter[] parameters = null)
        {
            if (parameters != null)
                this.Context.Database.ExecuteSqlRaw(commandText, (object[])parameters);
            else
                this.Context.Database.ExecuteSqlRaw(commandText);
        }

        public virtual int SaveChanges() => this.Context.SaveChanges();

        public virtual async Task<int> SaveChangesAsync()
        {
            int num = await this.Context.SaveChangesAsync(new CancellationToken());
            return num;
        }
    }
    public interface IGenericRepository<TEntity, TContext> where TEntity : class
    {
        TContext Context { get; }

        IQueryable<TEntity> Get(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          params Expression<Func<TEntity, object>>[] includeProperties);

        List<TEntity> GetFromStaticCache();

        TEntity GetByID(object id);

        Task<TEntity> GetByIDAsync(object id);

        IQueryable<TEntity> GetAll(bool Cacheable = false);

        Task<TEntity> FirstOrDefaultAsync(
          Expression<Func<TEntity, bool>> filter = null,
          params Expression<Func<TEntity, object>>[] includeProperties);

        void Insert(TEntity entity);

        void Update(TEntity entity, string[] properities);

        void Update(
          TEntity entityToUpdate,
          params Expression<Func<TEntity, object>>[] excludePropertyExpression);

        void UpdateField(
          TEntity entityToUpdate,
          params Expression<Func<TEntity, object>>[] includePropertyExpression);

        void AddRange(IEnumerable<TEntity> entities);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        void RemoveRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entitiy);

        IQueryable<TEntity> ExecuteReader(string sqlQuery, params object[] parametrs);

        void ExecuteNonQuery(string commandText, SqlParameter[] parameters = null);

        int SaveChanges();

        Task<int> SaveChangesAsync();

        IQueryable<TType> Cache<TType>(
          TimeSpan timeout,
          CacheExpirationMode cacheExpirationMode,
          Expression<Func<TEntity, TType>> selector = null,
          Expression<Func<TEntity, bool>> filter = null)
          where TType : class;
    }
}
