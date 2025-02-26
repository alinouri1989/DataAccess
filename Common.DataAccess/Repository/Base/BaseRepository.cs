using Common.Activation;
using Common.BaseDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Common.DataAccess.Repository.Base
{
    public class RepositoryBase<TEntity, TContext> : IRepositoryBase<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase()
        {
            _dbContext = ServiceActivator.ResolveService<TContext>(); ;
            _dbSet = _dbContext.Set<TEntity>();
        }
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
            return _dbSet.FromSqlInterpolated(sql);
        }

        public async Task<List<TEntity>> SqlQueryAsync(FormattableString sql)
        {
            return await _dbSet.FromSqlInterpolated(sql).ToListAsync();//.AsEnumerable();
        }
        public async Task<List<Tout>> SqlQueryAsync<Tout>(FormattableString sql)
        {
            return await _dbContext.Database.SqlQuery<Tout>(sql).ToListAsync();
        }
        #endregion

        #region Get  
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return orderBy != null ? orderBy(query) : query;
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }
        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public TEntity GetOne(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public TEntity GetOne(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Get(includeProperties: includeProperties).FirstOrDefault(predicate);
        }

        public async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity> GetOneAsNoTrackAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public TEntity GetOneAsNoTrack(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(predicate);
        }

        public async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await Get(includeProperties: includeProperties).FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> GetAnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public bool GetAny(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public async Task<List<TEntity>> GetSomeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public IQueryable<TEntity> GetSomeAsIQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public IQueryable<TEntity> GetAllAsIQueryable()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        #endregion

        #region Add  
        public async Task<TEntity> AddEntityAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<ResponseBaseDto<TEntity>> AddEntityByReponseAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return new ResponseBaseDto<TEntity> { Data = entity, Message = "Added successfully" };
        }

        public async Task<int> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> AddAllAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
            return entities.ToList();
        }

        // Synchronous methods  
        public TEntity AddEntity(TEntity entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public ResponseBaseDto<TEntity> AddEntityByReponse(TEntity entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
            return new ResponseBaseDto<TEntity> { Data = entity, Message = "Added successfully" };
        }

        public int Add(TEntity entity)
        {
            _dbSet.Add(entity);
            return _dbContext.SaveChanges();
        }

        public int AddAll(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            return _dbContext.SaveChanges();
        }

        public List<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            _dbContext.SaveChanges();
            return entities.ToList();
        }
        #endregion

        #region Update  
        public async Task<int> UpdateAsync(TEntity entity, bool withSave)
        {
            _dbSet.Update(entity);
            return withSave ? await _dbContext.SaveChangesAsync() : 0;
        }

        public async Task<int> UpdateFieldAsync(TEntity entity, bool withSave = true, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            _dbSet.Attach(entity);
            foreach (var includeProperty in includeProperties)
            {
                _dbContext.Entry(entity).Property(includeProperty).IsModified = true;
            }

            return withSave ? await _dbContext.SaveChangesAsync() : 0;
        }

        public async Task<int> UpdateFieldAsync(TEntity entity, bool withSave = true, params PropertyInfo[] includeProperties)
        {
            _dbSet.Attach(entity);
            foreach (var includeProperty in includeProperties)
            {
                _dbContext.Entry(entity).Property(includeProperty.Name).IsModified = true;
            }

            return withSave ? await _dbContext.SaveChangesAsync() : 0;
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateFieldAsync(TEntity entity, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            _dbSet.Attach(entity);
            foreach (var includeProperty in includeProperties)
            {
                _dbContext.Entry(entity).Property(includeProperty).IsModified = true;
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateFieldAsync(TEntity entity, params PropertyInfo[] includeProperties)
        {
            _dbSet.Attach(entity);
            foreach (var includeProperty in includeProperties)
            {
                _dbContext.Entry(entity).Property(includeProperty.Name).IsModified = true;
            }

            return await _dbContext.SaveChangesAsync();
        }

        // Synchronous methods  
        public int Update(TEntity entity, bool withSave)
        {
            _dbSet.Update(entity);
            return withSave ? _dbContext.SaveChanges() : 0;
        }

        public int UpdateField(TEntity entity, bool withSave = true, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            _dbSet.Attach(entity);
            foreach (var includeProperty in includeProperties)
            {
                _dbContext.Entry(entity).Property(includeProperty).IsModified = true;
            }

            return withSave ? _dbContext.SaveChanges() : 0;
        }

        public int UpdateField(TEntity entity, bool withSave = true, params PropertyInfo[] includeProperties)
        {
            _dbSet.Attach(entity);
            foreach (var includeProperty in includeProperties)
            {
                _dbContext.Entry(entity).Property(includeProperty.Name).IsModified = true;
            }

            return withSave ? _dbContext.SaveChanges() : 0;
        }

        public int Update(TEntity entity)
        {
            _dbSet.Update(entity);
            return _dbContext.SaveChanges();
        }

        public int UpdateField(TEntity entity, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            _dbSet.Attach(entity);
            foreach (var includeProperty in includeProperties)
            {
                _dbContext.Entry(entity).Property(includeProperty).IsModified = true;
            }

            return _dbContext.SaveChanges();
        }

        public int UpdateField(TEntity entity, params PropertyInfo[] includeProperties)
        {
            _dbSet.Attach(entity);
            foreach (var includeProperty in includeProperties)
            {
                _dbContext.Entry(entity).Property(includeProperty.Name).IsModified = true;
            }

            return _dbContext.SaveChanges();
        }
        #endregion

        #region Delete  
        public async Task<int> DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public int Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            return _dbContext.SaveChanges();
        }

        public int ForceDelete(TEntity entity)
        {
            _dbSet.Remove(entity);
            return _dbContext.SaveChanges();
        }

        public async Task<int> DeleteAllAsync()
        {
            var entities = await _dbSet.ToListAsync();
            _dbSet.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}