// Decompiled with JetBrains decompiler
// Type: CacheRepository.Service.Service`2
// Assembly: DataAccess, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6804894C-F989-4432-B8EA-6F3F70ACE424
// Assembly location: C:\Users\sa.nori\AppData\Local\Temp\Riqygac\68676b643f\lib\net5.0\DataAccess.dll

using Microsoft.Extensions.DependencyInjection;
using Repository;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CacheRepository.Service
{
  public abstract class Service<T, TContext> where T : class
  {
    private readonly IServiceProvider _serviceProvider;

    public Service(IServiceProvider serviceProvider) => this._serviceProvider = serviceProvider;

    public IGenericRepository<T, TContext> reopsitory => this.GetRepository<T, TContext>();

    public abstract IQueryable<T> Get(
      Expression<Func<T, bool>> filter = null,
      Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
      params Expression<Func<T, object>>[] includeProperties);

    public IGenericRepository<A, AContext> GetRepository<A, AContext>() where A : class => this._serviceProvider.GetRequiredService<IGenericRepository<A, AContext>>();

    public abstract T GetById(object Id);
  }
}
