using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeRegistration.BusinessLogic.Models
{
    public class MonthReport
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public CustomerReport[] Customers { get; set; }

        public TimeSpan TotalDuration
        {
            get
            {
                return TimeSpan.FromMinutes(Customers.Sum(x => x.TotalDuration.TotalMinutes));
            }
        }
    }
}
