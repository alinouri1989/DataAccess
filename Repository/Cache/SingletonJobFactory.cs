// Decompiled with JetBrains decompiler
// Type: Repository.Cache.SingletonJobFactory
// Assembly: DataAccess, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6804894C-F989-4432-B8EA-6F3F70ACE424
// Assembly location: C:\Users\sa.nori\AppData\Local\Temp\Riqygac\68676b643f\lib\net5.0\DataAccess.dll

using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

namespace Repository.Cache
{
  public class SingletonJobFactory : IJobFactory
  {
    private readonly IServiceProvider _serviceProvider;

    public SingletonJobFactory(IServiceProvider serviceProvider) => this._serviceProvider = serviceProvider;

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler) => this._serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;

    public void ReturnJob(IJob job)
    {
    }
  }
}
