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
