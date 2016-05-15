using System;

namespace TimeRegistration.BusinessLogic.Models
{
    public class Project
    {
        private readonly Guid projectId;
        private readonly string name;

        public Project(Guid projectId, string name)
        {
            this.projectId = projectId;
            this.name = name;
        }

        public Guid ProjectId => this.projectId;

        public string Name => this.name;
    }
}
