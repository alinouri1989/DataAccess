using Common.BaseDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Repository.Base
{
    public interface IRepositoryBase<TEntity, TContext>
                               where TEntity : class
                               where TContext : DbContext
    {

        #region SqlQuery
        IQueryable<TEntity> SqlQuery(FormattableString sql);
        Task<List<TEntity>> SqlQueryAsync(FormattableString sql);
        Task<List<Tout>> SqlQueryAsync<Tout>(FormattableString sql);
        #endregion


        #region Get
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(object id);
        TEntity GetById(object id);
        TEntity GetOne(Expression<Func<TEntity, bool>> predicate);
        TEntity GetOne(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetOneAsNoTrackAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity GetOneAsNoTrack(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<bool> GetAnyAsync(Expression<Func<TEntity, bool>> predicate);
        bool GetAny(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetSomeAsync(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetSomeAsIQueryable(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAllAsIQueryable();
        Task<List<TEntity>> GetAllAsync();
        #endregion

        #region Add
        Task<TEntity> AddEntityAsync(TEntity entity);
        Task<ResponseBaseDto<TEntity>> AddEntityByReponseAsync(TEntity entity);
        Task<int> AddAsync(TEntity entity);
        Task<int> AddAllAsync(IEnumerable<TEntity> entities);
        Task<List<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        // Synchronous methods  
        TEntity AddEntity(TEntity entity);
        ResponseBaseDto<TEntity> AddEntityByReponse(TEntity entity);
        int Add(TEntity entity);
        int AddAll(IEnumerable<TEntity> entities);
        List<TEntity> AddRange(IEnumerable<TEntity> entities);
        #endregion

        #region Update
        Task<int> UpdateAsync(TEntity entity, bool withSave);
        Task<int> UpdateFieldAsync(TEntity entity, bool withSave = true, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<int> UpdateFieldAsync(TEntity entity, bool withSave = true, params PropertyInfo[] includeProperties);

        Task<int> UpdateAsync(TEntity entity);
        Task<int> UpdateFieldAsync(TEntity entity, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<int> UpdateFieldAsync(TEntity entity, params PropertyInfo[] includeProperties);


        // Synchronous methods  
        int Update(TEntity entity, bool withSave);
        int UpdateField(TEntity entity, bool withSave = true, params Expression<Func<TEntity, object>>[] includeProperties);
        int UpdateField(TEntity entity, bool withSave = true, params PropertyInfo[] includeProperties);

        int Update(TEntity entity);
        int UpdateField(TEntity entity, params Expression<Func<TEntity, object>>[] includeProperties);
        int UpdateField(TEntity entity, params PropertyInfo[] includeProperties);
        #endregion

        #region Delete
        /// <summary>
        /// حذف فیزیکی
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(TEntity entity);
        int Delete(TEntity entity);
        int ForceDelete(TEntity entity);

        /// <summary>
        /// حذف همه رکوردها به صورت فیزیکی
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> DeleteAllAsync();
        #endregion
    }
}