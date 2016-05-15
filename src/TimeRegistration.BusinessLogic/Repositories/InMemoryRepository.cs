using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            CreateSampleData().Wait();
        }

// Async method lacks 'await' operators and will run synchronously. Not needed here, but will be used for real calls to DB
#pragma warning disable CS1998

        private async Task CreateSampleData()
        {
            this.customers.Add(new Customer(
                Guid.NewGuid(),
                "Danske Bank",
                new[]
                {
                    new Project(Guid.NewGuid(), "MobilePay"),
                    new Project(Guid.NewGuid(), "Netbank")
                }));

            this.customers.Add(new Customer(
                Guid.NewGuid(),
                "Københavns Lufthavn",
                new[]
                {
                    new Project(Guid.NewGuid(), "Kursus intranet")
                }));

            await AddRegistration(
                this.customers.First().CustomerId,
                this.customers.First().Projects.First().ProjectId,
                new Registration(
                    new DateTime(2015, 3, 2),
                    TimeSpan.FromHours(3),
                    "Did some awesome work"));

            await AddRegistration(
                this.customers.First().CustomerId,
                this.customers.First().Projects.First().ProjectId,
                new Registration(
                    new DateTime(2015, 3, 4),
                    TimeSpan.FromHours(5),
                    "Did some even more awesome work"));

            await AddRegistration(
                this.customers.First().CustomerId,
                this.customers.First().Projects.Last().ProjectId,
                new Registration(
                    new DateTime(2015, 3, 5),
                    TimeSpan.FromHours(0.5),
                    "Very little work on an old project"));

            await AddRegistration(
                this.customers.Last().CustomerId,
                this.customers.Last().Projects.First().ProjectId,
                new Registration(
                    new DateTime(2015, 3, 7),
                    TimeSpan.FromHours(1),
                    "Some telephone support to a previous project."));

            await AddRegistration(
                this.customers.First().CustomerId,
                this.customers.First().Projects.Last().ProjectId,
                new Registration(
                    new DateTime(2015, 5, 2),
                    TimeSpan.FromHours(10),
                    ""));

            await AddRegistration(
                this.customers.First().CustomerId,
                this.customers.First().Projects.Last().ProjectId,
                new Registration(
                    new DateTime(2016, 2, 15),
                    TimeSpan.FromHours(8),
                    ""));

            await AddRegistration(
                this.customers.Last().CustomerId,
                this.customers.Last().Projects.First().ProjectId,
                new Registration(
                    new DateTime(2016, 5, 1),
                    TimeSpan.FromHours(20),
                    "Many hours of development"));
        }

        public async Task<Customer[]> GetAllCustomers()
        {
            // Note: No locking needed as the customers list is read-only right now
            return this.customers.ToArray();
        }

        public async Task AddRegistration(Guid customerId, Guid projectId, Registration registration)
        {
            var customer = this.customers.FirstOrDefault(x => x.CustomerId == customerId);
            if (customer == null)
            {
                throw new ArgumentException("Customer not found", nameof(customerId));
            }

            var project = customer.Projects.FirstOrDefault(x => x.ProjectId == projectId);
            if (project == null)
            {
                throw new ArgumentException("Project not found on this customer", nameof(projectId));
            }

            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            if (registration.Duration.TotalMinutes <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(registration) + "." + nameof(registration.Duration), "Duration must at least 1 minute");
            }

            lock (this)
            {
                this.registrations.Add(new RegistrationWithContext(customerId, projectId, registration));
            }
        }

        public async Task<RegistrationWithContext[]> GetRegistrationsForDateRange(DateTime minDate, DateTime maxDate)
        {
            lock (this)
            {
                return this.registrations
                    .Where(x => x.Registration.Date >= minDate && x.Registration.Date <= maxDate)
                    .ToArray();
            }
        }

        public async Task<DateTime?> GetEarliestRegistrationDate()
        {
            lock (this)
            {
                return this.registrations.OrderBy(x => x.Registration.Date).FirstOrDefault()?.Registration.Date;
            }
        }

        public async Task<DateTime?> GetLatestRegistrationDate()
        {
            lock (this)
            {
                return this.registrations.OrderByDescending(x => x.Registration.Date).FirstOrDefault()?.Registration.Date;
            }
        }

#pragma warning restore CS1998
    }
}
