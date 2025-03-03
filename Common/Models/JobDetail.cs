namespace Common.Models
{
    public class JobDetail
    {
        public required string TypeName { get; set; }

        public required string JobKey { get; set; }
        public string? CronSchedule { get; set; }
        public int? IntervalMiliSeconds { get; set; }
        public int? WithIntervalInHours { get; set; }
        public bool IsActive { get; set; }
    }
}