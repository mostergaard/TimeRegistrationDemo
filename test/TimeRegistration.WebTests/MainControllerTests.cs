using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Moq;
using TimeRegistration.BusinessLogic.Interfaces;
using TimeRegistration.BusinessLogic.Models;
using TimeRegistration.Web.Controllers;
using TimeRegistration.Web.ViewModels.Main;
using Xunit;

namespace TimeRegistration.WebTests
{
    public class MainControllerTests
    {
        private Mock<IRepository> repositoryMock;
        private Mock<IReportGeneratorService> reportGeneratorMock;

        public MainControllerTests()
        {
            this.repositoryMock = new Mock<IRepository>();
            this.reportGeneratorMock = new Mock<IReportGeneratorService>();
        }

        private void ConfigureMocks(DateTime? earliestRegistrationDate, DateTime? latestRegistrationDate, int reportRequestYear, 
            int reportRequestMonth, MonthReport reportResponse)
        {
            this.repositoryMock.Setup(x => x.GetEarliestRegistrationDate()).Returns(Task.FromResult(earliestRegistrationDate));
            this.repositoryMock.Setup(x => x.GetLatestRegistrationDate()).Returns(Task.FromResult(latestRegistrationDate));
            this.reportGeneratorMock.Setup(x => x.GetMonthReport(reportRequestYear, reportRequestMonth)).Returns(Task.FromResult(reportResponse));
        }

        private async Task<MonthOverviewViewModel> ExecuteAction(int year = 0, int month = 0)
        {
            var controller = new MainController(this.repositoryMock.Object, this.reportGeneratorMock.Object);
            var viewResult = await controller.MonthOverview(year, month) as ViewResult;
            var viewModel = viewResult?.ViewData?.Model as MonthOverviewViewModel;
            return viewModel;
        }

        [Fact]
        public async Task Defaulting_to_current_month()
        {
            ConfigureMocks(DateTime.Today, DateTime.Today, DateTime.Today.Year, DateTime.Today.Month, new MonthReport());
            var viewModel = await ExecuteAction();

            Assert.Equal(DateTime.Today.Month, viewModel.SelectedMonth);
            Assert.Equal(DateTime.Today.Year, viewModel.SelectedYear);
            this.reportGeneratorMock.VerifyAll();
        }

        [Fact]
        public async Task Moves_to_first_used_month()
        {
            ConfigureMocks(new DateTime(2015, 3, 10), new DateTime(2015, 5, 20), 2015, 3, new MonthReport());
            var viewModel = await ExecuteAction(2015, 2);

            Assert.Equal(3, viewModel.SelectedMonth);
            Assert.Equal(2015, viewModel.SelectedYear);
            Assert.Equal(Enumerable.Range(3, 3), viewModel.PossibleMonths.Select(x => x.MonthNumber));
            Assert.Equal<string>(new[] { "2015" }, viewModel.PossibleYears.ToArray());
            this.reportGeneratorMock.VerifyAll();
        }

        [Fact]
        public async Task Moves_to_last_used_month()
        {
            ConfigureMocks(new DateTime(2015, 3, 10), new DateTime(2015, 5, 20), 2015, 5, new MonthReport());
            var viewModel = await ExecuteAction(2015, 6);

            Assert.Equal(5, viewModel.SelectedMonth);
            Assert.Equal(2015, viewModel.SelectedYear);
            Assert.Equal(Enumerable.Range(3, 3), viewModel.PossibleMonths.Select(x => x.MonthNumber));
            Assert.Equal<string>(new[] { "2015" }, viewModel.PossibleYears.ToArray());
            this.reportGeneratorMock.VerifyAll();
        }

        [Fact]
        public async Task Month_list_to_end_of_year()
        {
            ConfigureMocks(new DateTime(2015, 3, 10), new DateTime(2016, 3, 20), 2015, 5, new MonthReport());
            var viewModel = await ExecuteAction(2015, 5);

            Assert.Equal(5, viewModel.SelectedMonth);
            Assert.Equal(2015, viewModel.SelectedYear);
            Assert.Equal(Enumerable.Range(3, 10), viewModel.PossibleMonths.Select(x => x.MonthNumber));
            Assert.Equal<string>(new[] { "2015", "2016" }, viewModel.PossibleYears.ToArray());
            this.reportGeneratorMock.VerifyAll();
        }

        [Fact]
        public async Task Month_list_from_start_of_year()
        {
            ConfigureMocks(new DateTime(2015, 3, 10), new DateTime(2016, 3, 20), 2016, 2, new MonthReport());
            var viewModel = await ExecuteAction(2016, 2);

            Assert.Equal(2, viewModel.SelectedMonth);
            Assert.Equal(2016, viewModel.SelectedYear);
            Assert.Equal(Enumerable.Range(1, 3), viewModel.PossibleMonths.Select(x => x.MonthNumber));
            Assert.Equal<string>(new[] { "2015", "2016" }, viewModel.PossibleYears.ToArray());
            this.reportGeneratorMock.VerifyAll();
        }
    }
}
