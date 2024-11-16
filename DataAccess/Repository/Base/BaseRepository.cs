using Common.BaseDto;
using Common.BaseEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using San.CoreCommon.Constants;
using San.CoreCommon.ServiceActivator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Repository.Base
{
    public class RepositoryBase<TEntity, TContext> : BaseEf<TContext>, IRepositoryBase<TEntity, TContext> where TEntity : class where TContext : DbContext
    {
        #region Fields
        private readonly TContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public RepositoryBase()
        {
            _dbContext = ServiceActivator.ResolveService<TContext>();
            _dbSet = _dbContext.Set<TEntity>();
            _httpContextAccessor = ServiceActivator.CreateScope().ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        }
        #endregion


        public async Task<int> ExecuteInTransaction(Func<Task<int>> action)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var result = await action();
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                // Log exception here  
                await transaction.RollbackAsync();
                throw new Exception("error on ExecuteInTransaction(Func<Task<int>> action) on base repository", ex);
            }
        }


        /// <summary>
        /// excute one or more ection 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteInTransaction(Func<Task> action)
        {
            // Start a new transaction  
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await action(); // Execute the passed action  
                await _dbContext.SaveChangesAsync(); // Save changes to the database  
                await transaction.CommitAsync(); // Commit the transaction  

                return true; // Transaction successful  
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)  
                // await transaction.RollbackAsync(); // Rollback already handled by Dispose  
                throw new Exception("error on ExecuteInTransaction(Func<Task> action) on base repository", ex);
            }
        }

        #region SqlQuery
        public IQueryable<TEntity> SqlQuery(FormattableString sql)
        {
            return _dbSet.FromSqlInterpolated(sql);//.AsEnumerable();
        }
        public async Task<List<TEntity>> SqlQueryAsync(FormattableString sql)
        {
            return await _dbSet.FromSqlInterpolated(sql).ToListAsync();//.AsEnumerable();
        }
        public async Task<List<Tout>> SqlQueryAsync<Tout>(FormattableString sql)
        {
            return await _dbContext.Database.SqlQuery<Tout>(sql).ToListAsync();//.AsEnumerable();
        }
        #endregion

        #region Get

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = _dbSet;
            if (filter != null)
            {
                queryable = queryable.Where(filter);
            }

            foreach (Expression<Func<TEntity, object>> navigationPropertyPath in includeProperties)
            {
                queryable = EntityFrameworkQueryableExtensions.Include(queryable, navigationPropertyPath);
            }

            if (orderBy != null)
            {
                return orderBy(queryable);
            }

            return queryable;
        }

        public virtual async Task<TEntity> GetById(object id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetById on base repository", ex);
            }
        }
        public virtual async Task<TEntity> GetOneAsNoTrackAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetOneAsNoTrackAsync on base repository", ex);
            }
        }
        public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetOneAsync on base repository", ex);
            }
        }
        public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {

            try
            {
                var query = _dbSet.AsQueryable();


                if (includeProperties.Any())
                {
                    foreach (Expression<Func<TEntity, object>> navigationPropertyPath in includeProperties)
                    {
                        query = query.Include(navigationPropertyPath);
                    }
                }

                return await query.Where(predicate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetOneAsync on base repository", ex);
            }
        }
        public virtual TEntity GetOne(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate).FirstOrDefault();
        }
        public virtual TEntity GetOne(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query;
            try
            {
                if (predicate == null)
                {
                    query = _dbSet.AsQueryable();
                }
                else
                {
                    query = _dbSet.Where(predicate);
                }

                if (includeProperties.Any())
                {
                    foreach (Expression<Func<TEntity, object>> navigationPropertyPath in includeProperties)
                    {
                        query = query.Include(navigationPropertyPath);
                    }
                }

                return query.Where(predicate).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetOne on base repository", ex);
            }
        }

        public async Task<bool> GetAnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().AnyAsync(predicate);
        }

        public bool GetAny(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public virtual async Task<List<TEntity>> GetSome(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual IQueryable<TEntity> GetSomeAsIQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public virtual IQueryable<TEntity> GetAllAsIQueryable()
        {
            return _dbSet.AsNoTracking();
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
        #endregion

        #region Add
        public virtual async Task<TEntity> AddEntity(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await Commit();
            return entity;
        }
        public virtual async Task<ResponseBaseDto<TEntity>> AddEntityByReponse(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            var res = await Commit();
            if (res >= 1)
            {
                return new ResponseBaseDto<TEntity>
                {
                    Data = entity,
                    Message = "درج با موفقیت انجام شد",
                    Status = 0
                };
            }
            else
            {
                return new ResponseBaseDto<TEntity>
                {
                    Message = "خطا در درج",
                    Status = -1
                };
            }
        }

        public virtual async Task<int> Add(TEntity entity)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await _dbSet.AddAsync(entity);
                var result = await Commit();

                await transaction.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                // Log exception here  
                await transaction.RollbackAsync();
                throw new Exception("error on Add on base repository", ex);
            }
        }

        public virtual async Task<List<TEntity>> AddRange(IEnumerable<TEntity> entities)
        {
            try
            {
                await _dbContext.Set<TEntity>().AddRangeAsync(entities);
                this.UpdateAuditProps();

                await _dbContext.SaveChangesAsync();
                return entities.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("error on AddRange on base repository", ex);
            }
        }

        public virtual async Task<int> AddAll(IEnumerable<TEntity> entities)
        {
            try
            {
                await _dbContext.Set<TEntity>().AddRangeAsync(entities);
                this.UpdateAuditProps();

                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("error on AddAll on base repository", ex);
            }
        }
        #endregion

        #region Update

        #region [ Iman ]
        public virtual async Task<int> Update(TEntity entity, bool withSave = true)
        {
            try
            {
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
                return await CommitWithSave(withSave);
            }
            catch (Exception ex)
            {
                throw new Exception("error on Update on base repository", ex);
            }
        }

        public virtual async Task<int> UpdateField(TEntity entity, bool withSave = true, params PropertyInfo[] includeProperties)
        {

            var dbEntry = _dbContext.Entry(entity);
            bool flag = includeProperties != null;
            if (flag)
            {
                foreach (var includeProperty in includeProperties)
                {
                    dbEntry.Property(includeProperty.Name).IsModified = true;
                }
            }
            return await CommitWithSave(withSave);
        }

        public virtual async Task<int> UpdateField(TEntity entity, bool withSave = true, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var dbEntry = _dbContext.Entry(entity);
            bool flag = includeProperties != null;
            if (flag)
            {
                foreach (var includeProperty in includeProperties)
                {
                    dbEntry.Property(includeProperty).IsModified = true;
                }
            }

            return await CommitWithSave(withSave: withSave);
        }
        #endregion

        #region [ Ali ]
        public virtual async Task<int> Update(TEntity entity)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
                var result = await Commit();

                await transaction.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                // Log exception here  
                await transaction.RollbackAsync();
                throw new Exception("error on Update on base repository", ex);
            }

        }

        public virtual async Task<int> UpdateField(TEntity entity, params PropertyInfo[] includeProperties)
        {

            var dbEntry = _dbContext.Entry(entity);
            bool flag = includeProperties != null;
            if (flag)
            {
                foreach (var includeProperty in includeProperties)
                {
                    dbEntry.Property(includeProperty.Name).IsModified = true;
                }
            }
            return await Commit();
        }

        public virtual async Task<int> UpdateField(TEntity entity, params Expression<Func<TEntity, object>>[] includeProperties)
        {

            var dbEntry = _dbContext.Entry(entity);
            bool flag = includeProperties != null;
            if (flag)
            {
                foreach (var includeProperty in includeProperties)
                {
                    dbEntry.Property(includeProperty).IsModified = true;
                }
            }
            return await Commit();
        }
        #endregion
        #endregion

        #region Delete
        public virtual async Task<int> Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            return await Commit();
        }
        public virtual int ForceDelete(TEntity entity)
        {
            try
            {
                _dbSet.Remove(entity);
                return _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("error on ForceDelete on base repository", ex);
            }
        }
        public virtual async Task<int> DeleteAll()
        {
            var allItems = await _dbSet.ToListAsync();
            _dbSet.RemoveRange(allItems);
            return await Commit();
        }
        #endregion


        #region Commit
        protected virtual async Task<int> Commit(bool isUpdateAuditProps = true)//Ali nouri with transaction
        {
            try
            {
                if (isUpdateAuditProps)
                {
                    this.UpdateAuditProps();
                }

                var result = await _dbContext.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("error on commit on base repository", ex);
            }
        }

        /// <summary>
        /// ذخیره در پایگاه داده
        /// </summary>
        /// <param name="isUpdateAuditProps"></param>
        /// <param name="withSave"></param>
        /// <returns>0</returns>
        /// <returns>1</returns>
        /// <returns>-1</returns>
        protected virtual async Task<int> CommitWithSave(bool isUpdateAuditProps = true, bool withSave = true) // iman
        {
            try
            {
                if (isUpdateAuditProps)
                {
                    this.UpdateAuditProps();
                }

                if (withSave) return await _dbContext.SaveChangesAsync();

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("error on commitwithsave on base repository", ex);
            }
        }

        #endregion

        #region Private methods
        /// <summary>
        /// مقداردهی پراپرتی های پایه ای موجودیت ها
        /// </summary>
        private void UpdateAuditProps()
        {
            var userId = AuthenticationConsts.UserIdService;
            if (_httpContextAccessor.HttpContext != null)
                userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var entry in _dbContext.ChangeTracker.Entries<AuditEntityBase<int>>())
            {
                long.TryParse(userId, out long auditUserId);

                var auditDate = DateTime.Now;

                switch (entry.State)
                {
                    case EntityState.Detached:
                        break;

                    case EntityState.Unchanged:
                        break;

                    case EntityState.Added:
                        entry.Entity.CreateBy = auditUserId;
                        entry.Entity.CreateDate = auditDate;
                        break;

                    case EntityState.Deleted:
                        //entry.Entity.ModifierId = auditUserId;
                        //entry.Entity.ModifierIp = auditUserIp;
                        //entry.Entity.ModifierAgent = auditUserAgent;
                        //entry.Entity.ModifiedOn = auditDate;
                        //entry.Entity.DeletedOn = auditDate;
                        //entry.Entity.IsDeleted = true;
                        //entry.State = EntityState.Modified;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdateBy = auditUserId;
                        entry.Entity.UpdateDate = auditDate;
                        break;

                    default:
                        break;
                }
            }



            foreach (var entry in _dbContext.ChangeTracker.Entries<AuditEntityBase<long>>())
            {
                long.TryParse(userId, out long auditUserId);

                var auditDate = DateTime.Now;

                switch (entry.State)
                {
                    case EntityState.Detached:
                        break;

                    case EntityState.Unchanged:
                        break;

                    case EntityState.Added:
                        entry.Entity.CreateBy = auditUserId;
                        entry.Entity.CreateDate = auditDate;
                        break;

                    case EntityState.Deleted:
                        //entry.Entity.ModifierId = auditUserId;
                        //entry.Entity.ModifierIp = auditUserIp;
                        //entry.Entity.ModifierAgent = auditUserAgent;
                        //entry.Entity.ModifiedOn = auditDate;
                        //entry.Entity.IsDeleted = true;
                        //entry.State = EntityState.Modified;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdateBy = auditUserId;
                        entry.Entity.UpdateDate = auditDate;
                        break;

                    default:
                        break;
                }
            }
        }
        #endregion
    }
}