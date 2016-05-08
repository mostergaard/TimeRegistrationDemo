using System.Collections.Generic;
using TimeRegistration.BusinessLogic.Models;

namespace TimeRegistration.Web.ViewModels.Main
{
    public class PossibleMonth
    {
        public string Name { get; set; }

        public int MonthNumber { get; set; }
    }

    public class MonthOverviewViewModel
    {
        public IList<string> PossibleYears { get; set; }

        public IList<PossibleMonth> PossibleMonths { get; set; } 

        public int SelectedYear { get; set; }

        public int SelectedMonth { get; set; }

        public string SelectedMonthName { get; set; }

        public MonthReport Report { get; set; }
    }
}
