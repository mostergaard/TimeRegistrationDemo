using System;
using System.Linq;

namespace TimeRegistration.BusinessLogic.Models
{
    public class CustomerReport
    {
        public Guid CustomerId { get; set; }

        public string Name { get; set; }

        public ProjectReport[] Projects { get; set; }

        public TimeSpan TotalDuration
        {
            get
            {
                return TimeSpan.FromMinutes(Projects.Sum(x => x.TotalDuration.TotalMinutes));
            }
        }
    }
}
