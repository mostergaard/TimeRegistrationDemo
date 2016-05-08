using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TimeRegistration.BusinessLogic.Interfaces;
using TimeRegistration.BusinessLogic.Models;
using TimeRegistration.Web.ViewModels.Main;

namespace TimeRegistration.Web.Controllers
{
    public class MainController : Controller
    {
        private readonly IRepository repository;
        private readonly IReportGeneratorService reportGeneratorService;
        private readonly string[] monthNames;

        public MainController(IRepository repository, IReportGeneratorService reportGeneratorService)
        {
            this.repository = repository;
            this.reportGeneratorService = reportGeneratorService;

            this.monthNames = Enumerable.Range(1, 12).Select(x => new DateTime(2000, x, 1).ToString("MMMM", CultureInfo.InvariantCulture)).ToArray();
        }

        public IActionResult Default()
        {
            return RedirectToAction("MonthOverview");
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> MonthOverview(int year = 0, int month = 0)
        {
            if (year == 0)
            {
                year = DateTime.Today.Year;
            }

            if (month == 0)
            {
                month = DateTime.Today.Month;
            }

            var minDate = await this.repository.GetEarliestRegistrationDate();
            var maxDate = await this.repository.GetLatestRegistrationDate();

            // If we don't have any data, just return a empty report with the current month selected
            if (minDate == null || maxDate == null)
            {
                return View(new MonthOverviewViewModel
                {
                    PossibleYears = new[] { year.ToString() },
                    PossibleMonths = new[] { new PossibleMonth { MonthNumber = month, Name = this.monthNames[month - 1] } },
                    SelectedYear = year,
                    SelectedMonth = month,
                    SelectedMonthName = this.monthNames[month-1],
                    Report = null
                });
            }

            // Get date to view report for that is within the range of used dates
            var viewDate = new DateTime(year, month, 1);
            if (viewDate < minDate)
            {
                viewDate = minDate.Value;
            }
            else if (viewDate > maxDate)
            {
                viewDate = maxDate.Value;
            }

            // For the selected year, get the months to display
            var startMonth = new[] { minDate.Value, new DateTime(viewDate.Year, 1, 1) }.Max();
            var endMonth = new[] { maxDate.Value, new DateTime(viewDate.Year, 12, 1) }.Min();

            return View(new MonthOverviewViewModel
            {
                PossibleYears = Enumerable.Range(minDate.Value.Year, maxDate.Value.Year - minDate.Value.Year + 1)
                    .Select(x => x.ToString())
                    .ToArray(),
                PossibleMonths = Enumerable.Range(startMonth.Month, endMonth.Month - startMonth.Month + 1)
                    .Select(x => new PossibleMonth
                        {
                            Name = this.monthNames[x - 1],
                            MonthNumber = x
                        })
                    .ToArray(),
                SelectedYear = viewDate.Year,
                SelectedMonth = viewDate.Month,
                SelectedMonthName = this.monthNames[viewDate.Month - 1],
                Report = await this.reportGeneratorService.GetMonthReport(viewDate.Year, viewDate.Month)
            });
        }

        public async Task<IActionResult> Register()
        {
            // TODO: Add some jQuery magic on the page to refresh the list of possible projects per customer

            var customers = (await this.repository.GetAllCustomers()).ToArray();

            var model = new RegisterViewModel
            {
                CustomersAndProjects = customers,
                AllCustomers = customers,
                AllProjects = customers.SelectMany(x => x.Projects).ToList(),
                CustomerId = customers.First().CustomerId,
                ProjectId = customers.First().Projects.First().ProjectId,
                Date = DateTime.Today,
                Hours = 1,
                Notes = string.Empty
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                await this.repository.AddRegistration(model.CustomerId, model.ProjectId, new Registration
                {
                    Date = model.Date,
                    Duration = TimeSpan.FromHours(model.Hours),
                    Notes = model.Notes
                });

                return RedirectToAction("MonthOverview");
            }

            // We have lost the list of possible selections, so rebuild
            var customers = (await this.repository.GetAllCustomers()).ToArray();
            model.CustomersAndProjects = customers;
            model.AllCustomers = customers;
            model.AllProjects = customers.SelectMany(x => x.Projects).ToList();

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
