// Decompiled with JetBrains decompiler
// Type: Repository.Cache.JobSchedule
// Assembly: DataAccess, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6804894C-F989-4432-B8EA-6F3F70ACE424
// Assembly location: C:\Users\sa.nori\AppData\Local\Temp\Riqygac\68676b643f\lib\net5.0\DataAccess.dll

using Quartz;
using System;

namespace Repository.Cache
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
