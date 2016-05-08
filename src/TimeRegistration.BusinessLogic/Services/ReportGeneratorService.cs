using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeRegistration.BusinessLogic.Interfaces;
using TimeRegistration.BusinessLogic.Models;

namespace TimeRegistration.BusinessLogic.Services
{
    public class ReportGeneratorService : IReportGeneratorService
    {
        private readonly IRepository repository;

        public ReportGeneratorService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<MonthReport> GetMonthReport(int year, int month)
        {
            // Load data from repository
            var minDate = new DateTime(year, month, 1);
            var maxDate = minDate.AddMonths(1).AddDays(-1);
            var registrations = await this.repository.GetRegistrationsForDateRange(minDate, maxDate);
            var customers = await this.repository.GetAllCustomers();
            
            // Generate report
            var customerReports = new List<CustomerReport>();
            var groupedByCustomers = registrations.GroupBy(x => x.CustomerId);
            foreach (var customerGroup in groupedByCustomers)
            {
                var customer = customers.First(x => x.CustomerId == customerGroup.Key);

                var projectReports = customer.Projects.Select(project => new ProjectReport
                {
                    ProjectId = project.ProjectId,
                    Name = project.Name,
                    Registrations = customerGroup
                        .Where(x => x.ProjectId == project.ProjectId)
                        .Select(x => x.Registration)
                        .OrderBy(x => x.Date)
                        .ToArray()
                });

                customerReports.Add(new CustomerReport
                {
                    CustomerId = customer.CustomerId,
                    Name = customer.Name,
                    Projects = projectReports.Where(x => x.Registrations.Length > 0).ToArray()
                });
            }

            return new MonthReport
            {
                Year = year,
                Month = month,
                Customers = customerReports.Where(x => x.Projects.Length > 0).ToArray()
            };
        }
    }
}
