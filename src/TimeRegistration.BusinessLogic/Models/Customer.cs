using System;

namespace TimeRegistration.BusinessLogic.Models
{
    public class Customer
    {
        private readonly Guid customerId;
        private readonly string name;
        private readonly Project[] projects;

        public Customer(Guid customerId, string name, Project[] projects)
        {
            this.customerId = customerId;
            this.name = name;
            this.projects = projects;
        }

        public Guid CustomerId => this.customerId;

        public string Name => this.name;

        public Project[] Projects => this.projects;
    }
}
