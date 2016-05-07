using System;
using System.Collections.Generic;
using TimeRegistration.BusinessLogic.Interfaces;
using TimeRegistration.BusinessLogic.Models;

namespace TimeRegistration.BusinessLogic.Repositories
{
    public class InMemoryRepository : IRepository
    {
        private readonly List<Customer> customers = new List<Customer>();
        private readonly List<Tuple<Guid, Guid, Registration>> registrations = new List<Tuple<Guid, Guid, Registration>>();

        public InMemoryRepository()
        {
            CreateSampleData();
        }

        private void CreateSampleData()
        {
            this.customers.Add(new Customer
            {
                CustomerId = Guid.NewGuid(),
                Name = "Danske Bank",
                Projects = new[]
                {
                    new Project
                    {
                        ProjectId = Guid.NewGuid(),
                        Name = "MobilePay"
                    },
                    new Project
                    {
                        ProjectId = Guid.NewGuid(),
                        Name = "Netbank"
                    }
                }
            });

            this.customers.Add(new Customer
            {
                CustomerId = Guid.NewGuid(),
                Name = "Københavns Lufthavn",
                Projects = new[]
                            {
                    new Project
                    {
                        ProjectId = Guid.NewGuid(),
                        Name = "Kursus intranet"
                    }
                }
            });
        }
        
        public IEnumerable<Customer> GetAllCustomers()
        {
            return this.customers;
        }

        public void AddRegistration(Guid customerId, Guid projectId, Registration registration)
        {
            this.registrations.Add(new Tuple<Guid, Guid, Registration>(customerId, projectId, registration));
        }
    }
}
