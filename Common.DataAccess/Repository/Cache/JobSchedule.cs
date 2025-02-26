using Quartz;
using System;

namespace Common.DataAccess.Repository.Cache
{
  public class JobSchedule
  {
    public JobSchedule(Type jobType, Action<SimpleScheduleBuilder> schedule)
    {
      this.JobType = jobType;
      this.Schedule = schedule;
    }

    public Type JobType { get; }

    public Action<SimpleScheduleBuilder> Schedule { get; }
  }
}
