using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace Common.DataAccess.Repository.Cache
{
  public class QuartzHostedService : IHostedService
  {
    private readonly 
    #nullable disable
    ISchedulerFactory _schedulerFactory;
    private readonly IJobFactory _jobFactory;
    private readonly IEnumerable<JobSchedule> _jobSchedules;

    public QuartzHostedService(
      ISchedulerFactory schedulerFactory,
      IJobFactory jobFactory,
      IEnumerable<JobSchedule> jobSchedules)
    {
      this._schedulerFactory = schedulerFactory;
      this._jobSchedules = jobSchedules;
      this._jobFactory = jobFactory;
    }

    public IScheduler Scheduler { get; set; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
      IScheduler scheduler = await this._schedulerFactory.GetScheduler(cancellationToken);
      this.Scheduler = scheduler;
      scheduler = (IScheduler) null;
      this.Scheduler.JobFactory = this._jobFactory;
      foreach (JobSchedule jobSchedule in this._jobSchedules)
      {
        IJobDetail job = QuartzHostedService.CreateJob(jobSchedule);
        ITrigger trigger = QuartzHostedService.CreateTrigger(jobSchedule);
        DateTimeOffset dateTimeOffset = await this.Scheduler.ScheduleJob(job, trigger, cancellationToken);
        job = (IJobDetail) null;
        trigger = (ITrigger) null;
      }
      await this.Scheduler.Start(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken) => await this.Scheduler?.Shutdown(cancellationToken);

    private static IJobDetail CreateJob(JobSchedule schedule)
    {
      Type jobType = schedule.JobType;
      return JobBuilder.Create(jobType).WithIdentity(jobType.FullName).WithDescription(jobType.Name).Build();
    }

    private static ITrigger CreateTrigger(JobSchedule schedule) => TriggerBuilder.Create().WithIdentity(schedule.JobType.FullName + ".trigger").WithSimpleSchedule(schedule.Schedule).Build();
  }
}
