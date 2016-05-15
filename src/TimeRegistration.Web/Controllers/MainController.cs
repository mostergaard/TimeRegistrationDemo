using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TimeRegistration.BusinessLogic.Interfaces;
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

            this.monthNames = Enumerable.Range(1, 12)
                .Select(month => new DateTime(DateTime.Today.Year, month, 1).ToString("MMMM", CultureInfo.InvariantCulture))
                .ToArray();
        }

        [HttpGet("/")]
        public IActionResult Default()
        {
            return RedirectToAction("MonthOverview");
        }

        [HttpGet("/Error")]
        public IActionResult Error()
        {
            return View();
        }

        [HttpGet("/MonthOverview")]
        public async Task<IActionResult> MonthOverview(int year = 0, int month = 0)
        {
            // If no parameters, default to current month
            if (year == 0 || month == 0)
            {
                year = DateTime.Today.Year;
                month = DateTime.Today.Month;
            }

            // Get the first and last date we have registrations for
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

            // Make sure the report date is within the range of used dates - if not, move to the closest
            var viewDate = new DateTime(year, month, 1);
            if (viewDate < minDate)
            {
                viewDate = minDate.Value;
            }
            else if (viewDate > maxDate)
            {
                viewDate = maxDate.Value;
            }

            // For the selected year, get the range of months to display
            var firstMonth = MaxDate(minDate.Value, new DateTime(viewDate.Year, 1, 1)).Month;
            var lastMonth = MinDate(maxDate.Value, new DateTime(viewDate.Year, 12, 1)).Month;

            // Finally, generate the ViewModel
            return View(new MonthOverviewViewModel
            {
                PossibleYears = Enumerable.Range(minDate.Value.Year, maxDate.Value.Year - minDate.Value.Year + 1)
                    .Select(x => x.ToString())
                    .ToArray(),
                PossibleMonths = Enumerable.Range(firstMonth, lastMonth - firstMonth + 1)
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


        private DateTime MinDate(params DateTime[] items)
        {
            return items.Min();
        }

        private DateTime MaxDate(params DateTime[] items)
        {
            return items.Max();
        }
    }
}
