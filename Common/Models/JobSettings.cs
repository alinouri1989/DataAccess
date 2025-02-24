using System.Collections.Generic;

namespace SaleSystemService
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