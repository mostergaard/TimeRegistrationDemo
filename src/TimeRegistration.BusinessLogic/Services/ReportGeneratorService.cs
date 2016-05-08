using System;
using System.Collections.Generic;
using System.Linq;
using TimeRegistration.BusinessLogic.Interfaces;
using TimeRegistration.BusinessLogic.Models;

namespace TimeRegistration.BusinessLogic.Services
{
    public class ReportGeneratorService : IReportGeneratorService
    {
        private IRepository repository;

        public ReportGeneratorService(IRepository repository)
        {
            this.repository = repository;
        }

        public MonthReport GetMonthReport(int year, int month)
        {
            // Load data from repository
            var minDate = new DateTime(year, month, 1);
            var maxDate = minDate.AddMonths(1).AddDays(-1);
            var localRegistrations = this.repository.GetRegistrationsForDateTimeRange(minDate, maxDate);
            var localCustomers = this.repository.GetAllCustomers();
            
            // Generate report
            var customerReports = new List<CustomerReport>();
            var groupedByCustomers = localRegistrations.GroupBy(x => x.CustomerId);
            foreach (var customerGroup in groupedByCustomers)
            {
                var customer = localCustomers.First(x => x.CustomerId == customerGroup.Key);

                var projectReports = customer.Projects.Select(project => new ProjectReport
                {
                    ProjectId = project.ProjectId,
                    Name = project.Name,
                    Registrations = customerGroup
                        .Where(x => x.ProjectId == project.ProjectId)
                        .Select(x => x.Registration)
                        .ToArray()
                });

                customerReports.Add(new CustomerReport
                {
                    CustomerId = customer.CustomerId,
                    Name = customer.Name,
                    Projects = projectReports.ToArray()
                });
            }

            return new MonthReport
            {
                Year = year,
                Month = month,
                Customers = customerReports.ToArray()
            };
        }
    }
}
