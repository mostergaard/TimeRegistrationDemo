using TimeRegistration.BusinessLogic.Models;

namespace TimeRegistration.BusinessLogic.Interfaces
{
    public interface IReportGeneratorService
    {
        MonthReport GetMonthReport(int year, int month);
    }
}
