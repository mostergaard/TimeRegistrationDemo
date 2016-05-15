using System;
using System.Linq;

namespace TimeRegistration.BusinessLogic.Models
{
    public class ProjectReport
    {
        private readonly Guid projectId;
        private readonly string name;
        private readonly Registration[] registrations;

        public ProjectReport(Guid projectId, string name, Registration[] registrations)
        {
            this.projectId = projectId;
            this.name = name;
            this.registrations = registrations;
        }

        public Guid ProjectId => this.projectId;

        public string Name => this.name;

        public Registration[] Registrations => this.registrations;

        public TimeSpan TotalDuration => TimeSpan.FromMinutes(Registrations.Sum(x => x.Duration.TotalMinutes));
    }
}
