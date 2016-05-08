using System.Threading.Tasks;
using TimeRegistration.BusinessLogic.Models;

namespace TimeRegistration.BusinessLogic.Interfaces
{
    public interface IReportGeneratorService
    {
        Task<MonthReport> GetMonthReport(int year, int month);
    }
}
