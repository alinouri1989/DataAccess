using System.Collections.Generic;

namespace Common.Models
{
    public class JobSettings
    {
        public List<JobDetail> Jobs { get; set; }
        public JobSettings()
        {
            Jobs = new List<JobDetail>();
        }

    }

}