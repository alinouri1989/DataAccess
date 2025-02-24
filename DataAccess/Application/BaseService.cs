using AutoMapper;
using Common.BaseDto;
using DataAccess.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace DataAccess.Application.Base
{
    public class BaseService<TEntity, TContext> : IBaseService<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        public IRepositoryBase<TEntity, TContext> RepositoryBase { get; }
        public IMapper Mapper { get; }

        public BaseService(IRepositoryBase<TEntity, TContext> repositoryBase, IMapper mapper)
        {
            RepositoryBase = repositoryBase;
            Mapper = mapper;
        }

        public IQueryable<TDto> GetQueryable<TDto>()
        {
            var result = RepositoryBase.Get();
            return Mapper.ProjectTo<TDto>(result);
        }

        public async Task<List<TDto>> GetAllAsync<TDto>()
        {
            var result = await RepositoryBase.GetAllAsync();
            return Mapper.Map<List<TDto>>(result);
        }

        public async Task<List<TDto>> GetAllByExpressionAsync<TDto>(Expression<Func<TDto, bool>> predicate)
        {
            var mapfilter = Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
            var result = await RepositoryBase.GetSomeAsync(mapfilter);
            return Mapper.Map<List<TDto>>(result);
        }

        public TDto GetOne<TDto>(Expression<Func<TDto, bool>> filter)
        {
            var mapfilter = Mapper.Map<Expression<Func<TEntity, bool>>>(filter);
            var result = RepositoryBase.GetOne(mapfilter);
            return Mapper.Map<TDto>(result);
        }

        public async Task<TDto> GetByIdAsync<TDto>(long entityId)
        {
            var result = await RepositoryBase.GetByIdAsync(entityId);
            return Mapper.Map<TDto>(result);
        }

        public async Task<TDto> GetByIdAsync<TDto, Tkey>(Tkey entityId)
        {
            var result = await RepositoryBase.GetByIdAsync(entityId);
            return Mapper.Map<TDto>(result);
        }

        public async Task<ResponseBaseDto<int>> InsertAsync<TDto>(TDto dto)
        {
            try
            {
                var entity = Mapper.Map<TEntity>(dto);
                var res = await RepositoryBase.AddAsync(entity);
                return GetResponseBaseDto(res, "عملیات درج با موفقیت انجام شد", "خطا در عملیات درج");
            }
            catch (Exception)
            {
                return new ResponseBaseDto<int>
                {
                    Data = -1,
                    Message = "خطا در عملیات درج",
                    Status = -1
                };
            }
        }

        public async Task<ResponseBaseDto<TDto>> InsertEntityAsync<TDto>(TDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);
            var res = await RepositoryBase.AddEntityByReponseAsync(entity);
            if (res.Status == 0)
            {
                var inserted = Mapper.Map<TDto>(res.Data);
                return new ResponseBaseDto<TDto>
                {
                    Data = inserted,
                    Message = "عملیات درج با موفقیت انجام شد",
                    Status = 0
                };
            }
            return new ResponseBaseDto<TDto>
            {
                Message = "خطا در عملیات درج",
                Status = -1
            };
        }

        public async Task<ResponseBaseDto<int>> BulkInsertAsync<TDto>(IEnumerable<TDto> dtos)
        {
            var entities = Mapper.Map<List<TEntity>>(dtos);
            var res = await RepositoryBase.AddAllAsync(entities);
            return GetResponseBaseDto(res, "عملیات درج با موفقیت انجام شد", "خطا در عملیات درج");
        }

        public async Task<ResponseBaseDto<List<TDto>>> BulkInsertByGetEntitiesAsync<TDto>(IEnumerable<TDto> dtos)
        {
            var entities = Mapper.Map<List<TEntity>>(dtos);
            var res = await RepositoryBase.AddRangeAsync(entities);
            return res != null
                ? new ResponseBaseDto<List<TDto>>
                {
                    Data = Mapper.Map<List<TDto>>(res),
                    Message = "عملیات درج با موفقیت انجام شد",
                    Status = 0
                }
                : new ResponseBaseDto<List<TDto>>
                {
                    Data = null,
                    Message = "خطا در عملیات درج",
                    Status = -1
                };
        }

        public async Task<ResponseBaseDto<int>> RemoveAsync<TKey>(TKey entityId)
        {
            var model = await RepositoryBase.GetByIdAsync(entityId);
            var res = await RepositoryBase.DeleteAsync(model);
            return GetResponseBaseDto(res, "حذف با موفقیت انجام شد", "خطا در عملیات حذف");
        }

        public async Task<ResponseBaseDto<int>> ChangeStateAsync<TKey>(TKey entityId)
        {
            var model = await RepositoryBase.GetByIdAsync(entityId);
            var propertyInfo = model.GetType().GetProperty("IsActive");
            var isActive = (bool)propertyInfo.GetValue(model);

            propertyInfo.SetValue(model, !isActive, null);
            var res = await RepositoryBase.UpdateFieldAsync(model, includeProperties: propertyInfo);
            var message = isActive ? "تغییر وضعیت به غیر فعال تغییر یافت" : "تغییر وضعیت به فعال تغییر یافت";
            return GetResponseBaseDto(res, message, "خطا در عملیات غیر فعال سازی");
        }

        public IQueryable<T2Dto> GetToDto<T2Dto>(Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, dynamic>> sortBy = null,
            bool isSortAsc = true,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = filter != null
                ? RepositoryBase.GetSomeAsIQueryable(filter)
                : RepositoryBase.GetAllAsIQueryable();

            foreach (var includeProperty in includeProperties)
            {
                queryable = queryable.Include(includeProperty);
            }

            queryable = isSortAsc ? queryable.OrderBy(sortBy) : queryable.OrderByDescending(sortBy);
            return Mapper.ProjectTo<T2Dto>(queryable);
        }

        public T2Dto GetOneToDto<T2Dto>(Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, dynamic>> sortBy = null,
            bool isSortAsc = true,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var queryable = GetToDto<T2Dto>(filter, sortBy, isSortAsc, includeProperties);
            var result = queryable.FirstOrDefault();
            return Mapper.Map<T2Dto>(result);
        }

        public async Task<TDto> GetOneAsync<TDto>(Expression<Func<TDto, bool>> filter)
        {
            var mapfilter = Mapper.Map<Expression<Func<TEntity, bool>>>(filter);
            var result = await RepositoryBase.GetOneAsync(mapfilter);
            return Mapper.Map<TDto>(result);
        }

        public async Task<TDto> GetOneAsNoTrackAsync<TDto>(Expression<Func<TDto, bool>> predicate)
        {
            var mapfilter = Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
            var result = await RepositoryBase.GetOneAsNoTrackAsync(mapfilter);
            return Mapper.Map<TDto>(result);
        }

        public async Task<TDto> GetOneAsync<TDto>(Expression<Func<TDto, bool>> filter,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var mapfilter = Mapper.Map<Expression<Func<TEntity, bool>>>(filter);
            var result = includeProperties != null
                ? await RepositoryBase.GetOneAsync(mapfilter, includeProperties)
                : await RepositoryBase.GetOneAsync(mapfilter);
            return Mapper.Map<TDto>(result);
        }

        public async Task<ResponseBaseDto<int>> UpdateAsync<TDto>(TDto entityToUpdate, bool withSave = true)
        {
            var entity = Mapper.Map<TEntity>(entityToUpdate);
            var res = await RepositoryBase.UpdateAsync(entity, withSave);
            return GetResponseBaseDto(res, "عملیات ویرایش با موفقیت انجام شد", "خطا در عملیات ویرایش");
        }

        public async Task<ResponseBaseDto<int>> UpdateFieldAsync(TEntity entity, bool withSave = true, params PropertyInfo[] includeProperties)
        {
            var result = await RepositoryBase.UpdateFieldAsync(entity, withSave, includeProperties);
            return GetResponseBaseDto(result, "عملیات ویرایش با موفقیت انجام شد", "خطا در عملیات ویرایش");
        }

        public async Task<ResponseBaseDto<int>> UpdateFieldAsync(TEntity entity, bool withSave = true, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var result = await RepositoryBase.UpdateFieldAsync(entity, withSave, includeProperties);
            return GetResponseBaseDto(result, "عملیات ویرایش با موفقیت انجام شد", "خطا در عملیات ویرایش");
        }

        public async Task<ResponseBaseDto<int>> UpdateAsync<TDto>(TDto entityToUpdate)
        {
            var entity = Mapper.Map<TEntity>(entityToUpdate);
            var res = await RepositoryBase.UpdateAsync(entity);
            return GetResponseBaseDto(res, "عملیات ویرایش با موفقیت انجام شد", "خطا در عملیات ویرایش");
        }

        private ResponseBaseDto<int> GetResponseBaseDto(int response, string successMessage, string errorMessage)
        {
            return response > 0
                ? new ResponseBaseDto<int> { Data = response, Message = successMessage, Status = 0 }
                : new ResponseBaseDto<int> { Data = response, Message = errorMessage, Status = -1 };
        }

        public virtual async Task<bool> GetAnyAsync<TDto>(Expression<Func<TDto, bool>> filter)
        {
            var mapfilter = Mapper.Map<Expression<Func<TEntity, bool>>>(filter);
            return await RepositoryBase.GetAnyAsync(mapfilter);
        }

        public virtual bool GetAny<TDto>(Expression<Func<TDto, bool>> filter)
        {
            var mapfilter = Mapper.Map<Expression<Func<TEntity, bool>>>(filter);
            return RepositoryBase.GetAny(mapfilter);
        }

        // Synchronous method implementations  
        public ResponseBaseDto<int> Insert<TDto>(TDto dto)
        {
            try
            {
                var entity = Mapper.Map<TEntity>(dto);
                var res = RepositoryBase.Add(entity);
                return GetResponseBaseDto(res, "عملیات درج با موفقیت انجام شد", "خطا در عملیات درج");
            }
            catch (Exception)
            {
                return new ResponseBaseDto<int>
                {
                    Data = -1,
                    Message = "خطا در عملیات درج",
                    Status = -1
                };
            }
        }

        public ResponseBaseDto<TDto> InsertEntity<TDto>(TDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);
            var res = RepositoryBase.AddEntityByReponse(entity);
            if (res.Status == 0)
            {
                var inserted = Mapper.Map<TDto>(res.Data);
                return new ResponseBaseDto<TDto>
                {
                    Data = inserted,
                    Message = "عملیات درج با موفقیت انجام شد",
                    Status = 0
                };
            }
            return new ResponseBaseDto<TDto>
            {
                Message = "خطا در عملیات درج",
                Status = -1
            };
        }

        public ResponseBaseDto<int> BulkInsert<TDto>(IEnumerable<TDto> dtos)
        {
            var entities = Mapper.Map<List<TEntity>>(dtos);
            var res = RepositoryBase.AddAll(entities);
            return GetResponseBaseDto(res, "عملیات درج با موفقیت انجام شد", "خطا در عملیات درج");
        }

        public ResponseBaseDto<List<TDto>> BulkInsertByGetEntities<TDto>(IEnumerable<TDto> dtos)
        {
            var entities = Mapper.Map<List<TEntity>>(dtos);
            var res = RepositoryBase.AddRange(entities);
            return res != null
                ? new ResponseBaseDto<List<TDto>>
                {
                    Data = Mapper.Map<List<TDto>>(res),
                    Message = "عملیات درج با موفقیت انجام شد",
                    Status = 0
                }
                : new ResponseBaseDto<List<TDto>>
                {
                    Data = null,
                    Message = "خطا در عملیات درج",
                    Status = -1
                };
        }

        public ResponseBaseDto<int> ChangeState(long entityId)
        {
            var model = RepositoryBase.GetById(entityId);
            var propertyInfo = model.GetType().GetProperty("IsActive");
            var isActive = (bool)propertyInfo.GetValue(model);

            propertyInfo.SetValue(model, !isActive, null);
            var res = RepositoryBase.UpdateField(model, includeProperties: propertyInfo);
            var message = isActive ? "تغییر وضعیت به غیر فعال تغییر یافت" : "تغییر وضعیت به فعال تغییر یافت";
            return GetResponseBaseDto(res, message, "خطا در عملیات غیر فعال سازی");
        }

        public async Task<ResponseBaseDto<int>> ChangeStateAsync(long entityId)
        {
            var model = await RepositoryBase.GetByIdAsync(entityId);
            var propertyInfo = model.GetType().GetProperty("IsActive");
            var isActive = (bool)propertyInfo.GetValue(model);

            propertyInfo.SetValue(model, !isActive, null);
            var res = await RepositoryBase.UpdateFieldAsync(model, includeProperties: propertyInfo);
            var message = isActive ? "تغییر وضعیت به غیر فعال تغییر یافت" : "تغییر وضعیت به فعال تغییر یافت";
            return GetResponseBaseDto(res, message, "خطا در عملیات غیر فعال سازی");
        }

        public async Task<ResponseBaseDto<int>> UpdateFieldAsync(TEntity entity, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var result = await RepositoryBase.UpdateFieldAsync(entity, includeProperties);
            return GetResponseBaseDto(result, "عملیات ویرایش با موفقیت انجام شد", "خطا در عملیات ویرایش");
        }

        public async Task<ResponseBaseDto<int>> UpdateFieldAsync(TEntity entity, params PropertyInfo[] includeProperties)
        {
            var result = await RepositoryBase.UpdateFieldAsync(entity, includeProperties);
            return GetResponseBaseDto(result, "عملیات ویرایش با موفقیت انجام شد", "خطا در عملیات ویرایش");
        }
    }
}