using Common.BaseDto;
using Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Common
{
    public interface IBaseService<TEntity> : IBaseEf where TEntity : class
    {

        Task<List<TDto>> GetAll<TDto>();
        IQueryable<TDto> GetQueryable<TDto>();

        IQueryable<T2Dto> GetToDto<T2Dto>(Expression<Func<TEntity, bool>>? filter = null
                                        , Expression<Func<TEntity, dynamic>> sortBy = null
                                        , bool isSortAsc = true
                                        , params Expression<Func<TEntity, object>>[] includeProperties);

        T2Dto GetOneToDto<T2Dto>(Expression<Func<TEntity, bool>>? filter = null
                                , Expression<Func<TEntity, dynamic>> sortBy = null
                                , bool isSortAsc = true
                                , params Expression<Func<TEntity, object>>[] includeProperties);

        Task<List<TDto>> GetAllByExperssion<TDto>(Expression<Func<TDto, bool>> predicate);
        TDto GetOne<TDto>(Expression<Func<TDto, bool>> filter);
        Task<bool> GetAnyAsync<TDto>(Expression<Func<TDto, bool>> filter);
        bool GetAny<TDto>(Expression<Func<TDto, bool>> filter);
        Task<TDto> GetById<TDto>(long clubId);
        Task<TDto> GetById<TDto, Tkey>(Tkey clubId);
        Task<TDto> GetOneAsNoTrackAsync<TDto>(Expression<Func<TDto, bool>> predicate);

        Task<ResponseBaseDto<int>> Insert<TDto>(TDto dto);
        Task<ResponseBaseDto<TDto>> InsertEntity<TDto>(TDto dto);
        Task<ResponseBaseDto<int>> BulkInsert<TDto>(IEnumerable<TDto> dtos);
        Task<ResponseBaseDto<List<TDto>>> BulkInsertByGetEntities<TDto>(IEnumerable<TDto> dtos);
        Task<ResponseBaseDto<int>> ChangeState(long entityId);
        Task<ResponseBaseDto<int>> ChangeState<Tkey>(Tkey entityId);
        Task<ResponseBaseDto<int>> Remove<Tkey>(Tkey entityId);
        Task<TDto> GetOneAsync<TDto>(Expression<Func<TDto, bool>> filter);
        Task<TDto> GetOneAsync<TDto>(Expression<Func<TDto, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<ResponseBaseDto<int>> Update<TDto>(TDto entityToUpdate, bool withSave = true);
        Task<ResponseBaseDto<int>> UpdateField(TEntity entity, bool withSave = true, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<ResponseBaseDto<int>> UpdateField(TEntity entity, bool withSave = true, params PropertyInfo[] includeProperties);
        Task<ResponseBaseDto<int>> Update<TDto>(TDto entityToUpdate);
        Task<ResponseBaseDto<int>> UpdateField(TEntity entity, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<ResponseBaseDto<int>> UpdateField(TEntity entity, params PropertyInfo[] includeProperties);

    }
}
