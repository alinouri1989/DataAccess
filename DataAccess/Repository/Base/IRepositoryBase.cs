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
        Task<TEntity> GetById(object id);
        TEntity GetOne(Expression<Func<TEntity, bool>> predicate);
        TEntity GetOne(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetOneAsNoTrackAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<bool> GetAnyAsync(Expression<Func<TEntity, bool>> predicate);
        bool GetAny(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetSome(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetSomeAsIQueryable(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAllAsIQueryable();
        Task<List<TEntity>> GetAll();
        #endregion

        #region Add
        Task<TEntity> AddEntity(TEntity entity);
        Task<ResponseBaseDto<TEntity>> AddEntityByReponse(TEntity entity);
        Task<int> Add(TEntity entity);
        Task<int> AddAll(IEnumerable<TEntity> entities);
        Task<List<TEntity>> AddRange(IEnumerable<TEntity> entities);
        #endregion

        #region Update
        Task<int> Update(TEntity entity, bool withSave);
        Task<int> UpdateField(TEntity entity, bool withSave = true, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<int> UpdateField(TEntity entity, bool withSave = true, params PropertyInfo[] includeProperties);

        Task<int> Update(TEntity entity);
        Task<int> UpdateField(TEntity entity, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<int> UpdateField(TEntity entity, params PropertyInfo[] includeProperties);

        #endregion

        #region Delete
        /// <summary>
        /// حذف فیزیکی
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> Delete(TEntity entity);
        int ForceDelete(TEntity entity);

        /// <summary>
        /// حذف همه رکوردها به صورت فیزیکی
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> DeleteAll();
        #endregion
    }
}