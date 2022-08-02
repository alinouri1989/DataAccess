﻿// Decompiled with JetBrains decompiler
// Type: Repository.IGenericRepository`2
// Assembly: DataAccess, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6804894C-F989-4432-B8EA-6F3F70ACE424
// Assembly location: C:\Users\sa.nori\AppData\Local\Temp\Riqygac\68676b643f\lib\net5.0\DataAccess.dll

using EFCoreSecondLevelCacheInterceptor;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
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
