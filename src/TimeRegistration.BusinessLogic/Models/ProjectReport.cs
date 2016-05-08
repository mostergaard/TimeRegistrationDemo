using System;
using System.Linq;

namespace TimeRegistration.BusinessLogic.Models
{
    public class ProjectReport
    {
        public Guid ProjectId { get; set; }

        public string Name { get; set; }

        public Registration[] Registrations { get; set; }

        public TimeSpan TotalDuration
        {
            get
            {
                return TimeSpan.FromMinutes(Registrations.Sum(x => x.Duration.TotalMinutes));
            }
        }
    }
}
