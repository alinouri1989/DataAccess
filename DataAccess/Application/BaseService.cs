using AutoMapper;
using Common.BaseDto;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Common
{
    public class BaseService<TEntity, TContext> : IBaseService<TEntity, TContext> where TEntity : class
                                                                                  where TContext : DbContext
    {
        public IRepositoryBase<TEntity, TContext> RepositoryBase { get; }
        public IMapper Mapper { get; }

        public BaseService(IRepositoryBase<TEntity, TContext> repositoryBase,
            DbContext dbContext,
            IMapper mapper)
        {
            RepositoryBase = repositoryBase;
            Mapper = mapper;
        }
        public IQueryable<TDto> GetQueryable<TDto>()
        {
            var result = RepositoryBase.Get();
            return Mapper.ProjectTo<TDto>(result);
        }

        public async Task<List<TDto>> GetAll<TDto>()
        {
            var result = await RepositoryBase.GetAll();

            return Mapper.Map<List<TDto>>(result);
        }

        public async Task<List<TDto>> GetAllByExperssion<TDto>(Expression<Func<TDto, bool>> predicate)
        {
            var mapfilter = Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);

            var result = await RepositoryBase.GetSome(mapfilter);

            return Mapper.Map<List<TDto>>(result);
        }

        public TDto GetOne<TDto>(Expression<Func<TDto, bool>> filter)
        {
            var mapfilter = Mapper.Map<Expression<Func<TEntity, bool>>>(filter);

            var result = RepositoryBase.GetOne(mapfilter);

            return Mapper.Map<TDto>(result);
        }

        public async Task<TDto> GetById<TDto>(long entityId)
        {
            var result = await RepositoryBase.GetById(entityId);

            return Mapper.Map<TDto>(result);
        }

        public async Task<TDto> GetById<TDto, Tkey>(Tkey entityId)
        {
            var result = await RepositoryBase.GetById(entityId);

            return Mapper.Map<TDto>(result);
        }

        public async Task<ResponseBaseDto<int>> Insert<TDto>(TDto dto)
        {
            try
            {
                var entity = Mapper.Map<TEntity>(dto);
                var res = await RepositoryBase.Add(entity);
                if (res > 0)
                {
                    return new ResponseBaseDto<int>
                    {
                        Data = res,
                        Message = $"با موفقیت انجام شد",
                        Status = 0
                    };
                }
                else
                {
                    return new ResponseBaseDto<int>
                    {
                        Data = -1,
                        Message = $"خطا در عملیات درج",
                        Status = -1
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseBaseDto<int>
                {
                    Data = -1,
                    Message = $"خطا در عملیات درج",
                    Status = -1
                };
            }
        }
        public async Task<ResponseBaseDto<TDto>> InsertEntity<TDto>(TDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);
            var res = await RepositoryBase.AddEntityByReponse(entity);
            if (res.Status == 0)
            {
                var inserted = Mapper.Map<TDto>(res.Data);
                if (inserted != null)
                {
                    return new ResponseBaseDto<TDto>
                    {
                        Data = inserted,
                        Message = "عملیات درج با موفقیت انجام شد",
                        Status = 0
                    };
                }
                else
                {
                    return new ResponseBaseDto<TDto>
                    {
                        Message = "خطا در عملیات درج",
                        Status = -1
                    };
                }
            }
            else
            {
                return new ResponseBaseDto<TDto>
                {
                    Message = "خطا در عملیات درج",
                    Status = -1
                };
            }
        }
        public async Task<ResponseBaseDto<int>> BulkInsert<TDto>(IEnumerable<TDto> dtos)
        {
            var entities = Mapper.Map<List<TEntity>>(dtos);
            var res = await RepositoryBase.AddAll(entities);
            if (res > 0)
            {
                return new ResponseBaseDto<int>
                {
                    Data = res,
                    Message = $"با موفقیت انجام شد",
                    Status = 0
                };
            }
            else
            {
                return new ResponseBaseDto<int>
                {
                    Data = res,
                    Message = $"خطا در عملیات درج",
                    Status = -1
                };
            }
        }
        public async Task<ResponseBaseDto<List<TDto>>> BulkInsertByGetEntities<TDto>(IEnumerable<TDto> dtos)
        {
            var entities = Mapper.Map<List<TEntity>>(dtos);
            var res = await RepositoryBase.AddRange(entities);
            if (res != null)
            {
                return new ResponseBaseDto<List<TDto>>
                {
                    Data = Mapper.Map<List<TDto>>(res),
                    Message = $"با موفقیت انجام شد",
                    Status = 0
                };
            }
            else
            {
                return new ResponseBaseDto<List<TDto>>
                {
                    Data = null,
                    Message = $"خطا در عملیات درج",
                    Status = -1
                };
            }
        }
        public async Task<ResponseBaseDto<int>> Remove<TKey>(TKey entityId)
        {
            var model = await RepositoryBase.GetById(entityId);

            var res = await RepositoryBase.Delete(model);
            if (res > 0)
            {
                return new ResponseBaseDto<int>
                {
                    Data = res,
                    Message = "حذف با موفقیت انجام شد",
                    Status = 0
                };
            }
            else
            {
                return new ResponseBaseDto<int>
                {
                    Data = res,
                    Message = $"خطا در عملیات حذف",
                    Status = 0
                };
            }
        }
        public async Task<ResponseBaseDto<int>> ChangeState<TKey>(TKey entityId)
        {
            var model = await RepositoryBase.GetById(entityId);

            var type = model.GetType().GetProperties();

            string _message;

            PropertyInfo propertyInfo = type.FirstOrDefault(p => p.Name == "IsActive");
            if ((bool)propertyInfo.GetValue(model))
            {
                propertyInfo.SetValue(model, false, null);
                _message = "تغییر وضعیت به غیر فعال تغییر یافت";
            }
            else
            {
                propertyInfo.SetValue(model, true, null);
                _message = "تغییر وضعیت به فعال تغییر یافت";
            }


            var res = await RepositoryBase.UpdateField(model, includeProperties: propertyInfo);
            if (res > 0)
            {
                return new ResponseBaseDto<int>
                {
                    Data = res,
                    Message = _message,
                    Status = 0
                };
            }
            else
            {
                return new ResponseBaseDto<int>
                {
                    Data = res,
                    Message = $"خطا در عملیات غیر فعال سازی",
                    Status = 0
                };
            }
        }
        public async Task<ResponseBaseDto<int>> ChangeState(long entityId)
        {
            var model = await RepositoryBase.GetById(entityId);

            var type = model.GetType().GetProperties();

            string _message;

            PropertyInfo propertyInfo = type.FirstOrDefault(p => p.Name == "IsActive");
            if ((bool)propertyInfo.GetValue(model))
            {
                propertyInfo.SetValue(model, false, null);
                _message = "تغییر وضعیت به غیر فعال تغییر یافت";
            }
            else
            {
                propertyInfo.SetValue(model, true, null);
                _message = "تغییر وضعیت به فعال تغییر یافت";
            }


            var res = await RepositoryBase.UpdateField(model, includeProperties: propertyInfo);
            if (res > 0)
            {
                return new ResponseBaseDto<int>
                {
                    Data = res,
                    Message = _message,
                    Status = 0
                };
            }
            else
            {
                return new ResponseBaseDto<int>
                {
                    Data = res,
                    Message = $"خطا در عملیات غیر فعال سازی",
                    Status = 0
                };
            }
        }


        public IQueryable<T2Dto> GetToDto<T2Dto>(Expression<Func<TEntity, bool>> filter = null
            , Expression<Func<TEntity, dynamic>> sortBy = null
            , bool isSortAsc = true
            , params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable;
            if (filter != null)
            {
                queryable = RepositoryBase.GetSomeAsIQueryable(filter);
            }
            else
            {
                queryable = RepositoryBase.GetAllAsIQueryable();
            }
            if (includeProperties != null)
            {
                foreach (Expression<Func<TEntity, object>> navigationPropertyPath in includeProperties)
                {
                    queryable = EntityFrameworkQueryableExtensions.Include(queryable, navigationPropertyPath);
                }
            }
            if (sortBy != null)
            {
                if (isSortAsc)
                    queryable = queryable.OrderBy(sortBy);
                else
                    queryable = queryable.OrderByDescending(sortBy);
            }


            return Mapper.ProjectTo<T2Dto>(queryable);
        }

        public T2Dto GetOneToDto<T2Dto>(Expression<Func<TEntity, bool>> filter = null
        , Expression<Func<TEntity, dynamic>> sortBy = null
        , bool isSortAsc = true
        , params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable;
            if (filter != null)
            {
                queryable = RepositoryBase.GetSomeAsIQueryable(filter);
            }
            else
            {
                queryable = RepositoryBase.GetAllAsIQueryable();
            }
            if (includeProperties != null)
            {
                foreach (Expression<Func<TEntity, object>> navigationPropertyPath in includeProperties)
                {
                    queryable = EntityFrameworkQueryableExtensions.Include(queryable, navigationPropertyPath);
                }
            }
            if (sortBy != null)
            {
                if (isSortAsc)
                    queryable = queryable.OrderBy(sortBy);
                else
                    queryable = queryable.OrderByDescending(sortBy);
            }

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

        public async Task<TDto> GetOneAsync<TDto>(Expression<Func<TDto, bool>> filter
            , params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var mapfilter = Mapper.Map<Expression<Func<TEntity, bool>>>(filter);
            if (includeProperties != null)
            {
                var result = await RepositoryBase.GetOneAsync(mapfilter, includeProperties);
                return Mapper.Map<TDto>(result);
            }
            else
            {
                var result = await RepositoryBase.GetOneAsync(mapfilter);
                return Mapper.Map<TDto>(result);
            }
        }

        public async Task<ResponseBaseDto<int>> Update<TDto>(TDto entityToUpdate, bool withSave = true)
        {
            var entity = Mapper.Map<TEntity>(entityToUpdate);
            var res = await RepositoryBase.Update(entity, withSave);
            if (res > 0)
            {
                return new ResponseBaseDto<int>
                {
                    Data = res,
                    Message = $"عملیات ویرایش با موفقیت انجام شد",
                    Status = 0
                };
            }
            else
            {
                return new ResponseBaseDto<int>
                {
                    Data = res,
                    Message = $"خطا در عملیات ویرایش",
                    Status = 0
                };
            }
        }
        public virtual async Task<ResponseBaseDto<int>> UpdateField(TEntity entity, bool withSave = true, params PropertyInfo[] includeProperties)
        {

            var result = await RepositoryBase.UpdateField(entity, withSave, includeProperties: includeProperties);
            if (result > 0)
            {
                return new ResponseBaseDto<int>
                {
                    Data = result,
                    Message = $"عملیات ویرایش با موفقیت انجام شد",
                    Status = 0
                };
            }
            else
            {
                return new ResponseBaseDto<int>
                {
                    Data = result,
                    Message = $"خطا در عملیات ویرایش",
                    Status = -1
                };
            }
        }

        public virtual async Task<ResponseBaseDto<int>> UpdateField(TEntity entity, bool withSave = true, params Expression<Func<TEntity, object>>[] includeProperties)
        {

            var result = await RepositoryBase.UpdateField(entity, withSave, includeProperties: includeProperties);

            if (result > 0)
            {
                return new ResponseBaseDto<int>
                {
                    Data = result,
                    Message = $"عملیات ویرایش با موفقیت انجام شد",
                    Status = 0
                };
            }
            else
            {
                return new ResponseBaseDto<int>
                {
                    Data = result,
                    Message = $"خطا در عملیات ویرایش",
                    Status = -1
                };
            }
        }

        public async Task<ResponseBaseDto<int>> Update<TDto>(TDto entityToUpdate)
        {
            var entity = Mapper.Map<TEntity>(entityToUpdate);
            var res = await RepositoryBase.Update(entity);
            if (res > 0)
            {
                return new ResponseBaseDto<int>
                {
                    Data = res,
                    Message = $"عملیات ویرایش با موفقیت انجام شد",
                    Status = 0
                };
            }
            else
            {
                return new ResponseBaseDto<int>
                {
                    Data = res,
                    Message = $"خطا در عملیات ویرایش",
                    Status = 0
                };
            }
        }
        public virtual async Task<ResponseBaseDto<int>> UpdateField(TEntity entity, params PropertyInfo[] includeProperties)
        {

            var result = await RepositoryBase.UpdateField(entity, includeProperties: includeProperties);
            if (result > 0)
            {
                return new ResponseBaseDto<int>
                {
                    Data = result,
                    Message = $"عملیات ویرایش با موفقیت انجام شد",
                    Status = 0
                };
            }
            else
            {
                return new ResponseBaseDto<int>
                {
                    Data = result,
                    Message = $"خطا در عملیات ویرایش",
                    Status = -1
                };
            }
        }

        public virtual async Task<ResponseBaseDto<int>> UpdateField(TEntity entity, params Expression<Func<TEntity, object>>[] includeProperties)
        {

            var result = await RepositoryBase.UpdateField(entity, includeProperties: includeProperties);

            if (result > 0)
            {
                return new ResponseBaseDto<int>
                {
                    Data = result,
                    Message = $"عملیات ویرایش با موفقیت انجام شد",
                    Status = 0
                };
            }
            else
            {
                return new ResponseBaseDto<int>
                {
                    Data = result,
                    Message = $"خطا در عملیات ویرایش",
                    Status = -1
                };
            }
        }



        public virtual async Task<bool> GetAnyAsync<TDto>(Expression<Func<TDto, bool>> filter)
        {
            var mapfilter = Mapper.Map<Expression<Func<TEntity, bool>>>(filter);

            var result = await RepositoryBase.GetAnyAsync(mapfilter);
            return result;
        }

        public virtual bool GetAny<TDto>(Expression<Func<TDto, bool>> filter)
        {
            var mapfilter = Mapper.Map<Expression<Func<TEntity, bool>>>(filter);

            var result = RepositoryBase.GetAny(mapfilter);
            return result;
        }
    }
}
