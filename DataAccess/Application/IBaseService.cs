using Common.BaseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace DataAccess.Application.Base
{
    public interface IBaseService<TEntity, TContext> where TEntity : class
    {
        // Asynchronous methods  
        Task<List<TDto>> GetAllAsync<TDto>();
        Task<List<TDto>> GetAllByExpressionAsync<TDto>(Expression<Func<TDto, bool>> predicate);
        Task<TDto> GetByIdAsync<TDto>(long entityId);
        Task<TDto> GetByIdAsync<TDto, Tkey>(Tkey entityId);
        Task<TDto> GetOneAsync<TDto>(Expression<Func<TDto, bool>> filter);
        Task<TDto> GetOneAsync<TDto>(Expression<Func<TDto, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<bool> GetAnyAsync<TDto>(Expression<Func<TDto, bool>> filter);

        // Synchronous methods  
        IQueryable<TDto> GetQueryable<TDto>();
        TDto GetOne<TDto>(Expression<Func<TDto, bool>> filter);
        bool GetAny<TDto>(Expression<Func<TDto, bool>> filter);
        IQueryable<T2Dto> GetToDto<T2Dto>(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, dynamic>> sortBy = null,
            bool isSortAsc = true,
            params Expression<Func<TEntity, object>>[] includeProperties);
        T2Dto GetOneToDto<T2Dto>(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, dynamic>> sortBy = null,
            bool isSortAsc = true,
            params Expression<Func<TEntity, object>>[] includeProperties);

        // Insert Methods  
        Task<ResponseBaseDto<int>> InsertAsync<TDto>(TDto dto);
        ResponseBaseDto<int> Insert<TDto>(TDto dto);
        Task<ResponseBaseDto<TDto>> InsertEntityAsync<TDto>(TDto dto);
        ResponseBaseDto<TDto> InsertEntity<TDto>(TDto dto);
        Task<ResponseBaseDto<int>> BulkInsertAsync<TDto>(IEnumerable<TDto> dtos);
        ResponseBaseDto<int> BulkInsert<TDto>(IEnumerable<TDto> dtos);
        Task<ResponseBaseDto<List<TDto>>> BulkInsertByGetEntitiesAsync<TDto>(IEnumerable<TDto> dtos);
        ResponseBaseDto<List<TDto>> BulkInsertByGetEntities<TDto>(IEnumerable<TDto> dtos);

        // Change State Methods  
        Task<ResponseBaseDto<int>> ChangeStateAsync(long entityId);
        Task<ResponseBaseDto<int>> ChangeStateAsync<Tkey>(Tkey entityId);

        // Remove Methods  
        Task<ResponseBaseDto<int>> RemoveAsync<Tkey>(Tkey entityId);

        // Update Methods  
        Task<ResponseBaseDto<int>> UpdateAsync<TDto>(TDto entityToUpdate, bool withSave = true);
        Task<ResponseBaseDto<int>> UpdateFieldAsync(TEntity entity, bool withSave = true, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<ResponseBaseDto<int>> UpdateFieldAsync(TEntity entity, bool withSave = true, params PropertyInfo[] includeProperties);
        Task<ResponseBaseDto<int>> UpdateAsync<TDto>(TDto entityToUpdate);
        Task<ResponseBaseDto<int>> UpdateFieldAsync(TEntity entity, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<ResponseBaseDto<int>> UpdateFieldAsync(TEntity entity, params PropertyInfo[] includeProperties);
    }
}