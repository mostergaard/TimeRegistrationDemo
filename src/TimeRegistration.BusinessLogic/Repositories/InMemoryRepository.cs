using System;
using System.Collections.Generic;
using System.Linq;
using TimeRegistration.BusinessLogic.Interfaces;
using TimeRegistration.BusinessLogic.Models;

namespace TimeRegistration.BusinessLogic.Repositories
{
    public class InMemoryRepository : IRepository
    {
        private readonly List<Customer> customers = new List<Customer>();
        private readonly List<RegistrationWithContext> registrations = new List<RegistrationWithContext>();

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

            AddRegistration(
                this.customers.First().CustomerId, 
                this.customers.First().Projects.First().ProjectId,
                new Registration
                {
                    Date = new DateTime(2015, 3, 2),
                    Duration = TimeSpan.FromHours(3),
                    Notes = "Did some awesome work"
                });

            AddRegistration(
                this.customers.First().CustomerId,
                this.customers.First().Projects.First().ProjectId,
                new Registration
                {
                    Date = new DateTime(2015, 3, 4),
                    Duration = TimeSpan.FromHours(5),
                    Notes = "Did some even more awesome work"
                });

            AddRegistration(
                this.customers.First().CustomerId,
                this.customers.First().Projects.Last().ProjectId,
                new Registration
                {
                    Date = new DateTime(2015, 3, 5),
                    Duration = TimeSpan.FromHours(0.5),
                    Notes = "Very little work on an old project"
                });

            AddRegistration(
                this.customers.Last().CustomerId,
                this.customers.Last().Projects.First().ProjectId,
                new Registration
                {
                    Date = new DateTime(2015, 3, 7),
                    Duration = TimeSpan.FromHours(1),
                    Notes = "Some telephone support to a previous project."
                });

            AddRegistration(
                this.customers.First().CustomerId,
                this.customers.First().Projects.Last().ProjectId,
                new Registration
                {
                    Date = new DateTime(2015, 5, 2),
                    Duration = TimeSpan.FromHours(10)
                });

            AddRegistration(
                this.customers.First().CustomerId,
                this.customers.First().Projects.Last().ProjectId,
                new Registration
                {
                    Date = new DateTime(2016, 2, 15),
                    Duration = TimeSpan.FromHours(8)
                });

            AddRegistration(
                this.customers.Last().CustomerId,
                this.customers.Last().Projects.First().ProjectId,
                new Registration
                {
                    Date = new DateTime(2016, 5, 1),
                    Duration = TimeSpan.FromHours(20),
                    Notes = "Many hours of development"
                });
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            // Note: No locking needed as the customers list is read-only right now
            return this.customers;
        }

        public void AddRegistration(Guid customerId, Guid projectId, Registration registration)
        {
            // TODO: Validate that the customer id and project id are valid and that the project id exists under the customer!
            lock (this)
            {
                this.registrations.Add(new RegistrationWithContext
                {
                    CustomerId = customerId,
                    ProjectId = projectId,
                    Registration = registration
                });
            }
        }

        public IList<RegistrationWithContext> GetRegistrationsForDateTimeRange(DateTime minDate, DateTime maxDate)
        {
            RegistrationWithContext[] result;
            lock (this)
            {
                result = this.registrations
                    .Where(x => x.Registration.Date >= minDate && x.Registration.Date <= maxDate)
                    .ToArray();
            }

            return result;
        }

        public DateTime? GetEarliestRegistrationDate()
        {
            lock (this)
            {
                return this.registrations.OrderBy(x => x.Registration.Date).FirstOrDefault()?.Registration.Date;
            }
        }

        public DateTime? GetLatestRegistrationDate()
        {
            lock (this)
            {
                return this.registrations.OrderByDescending(x => x.Registration.Date).FirstOrDefault()?.Registration.Date;
            }
        }
    }
}
