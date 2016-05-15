using System;
using System.Linq;

namespace TimeRegistration.BusinessLogic.Models
{
    public class CustomerReport
    {
        private readonly Guid customerId;
        private readonly string name;
        private readonly ProjectReport[] projects;

        public CustomerReport(Guid customerId, string name, ProjectReport[] projects)
        {
            this.customerId = customerId;
            this.name = name;
            this.projects = projects;
        }

        public Guid CustomerId => this.customerId;

        public string Name => this.name;

        public ProjectReport[] Projects => this.projects;

        public TimeSpan TotalDuration => TimeSpan.FromMinutes(Projects.Sum(x => x.TotalDuration.TotalMinutes));
    }
}
