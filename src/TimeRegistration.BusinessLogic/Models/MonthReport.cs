using System;
using System.Linq;

namespace TimeRegistration.BusinessLogic.Models
{
    public class MonthReport
    {
        private readonly int year;
        private readonly int month;
        private readonly CustomerReport[] customers;

        public MonthReport(int year, int month, CustomerReport[] customers)
        {
            this.year = year;
            this.month = month;
            this.customers = customers;
        }

        public int Year => this.year;

        public int Month => this.month;

        public CustomerReport[] Customers => this.customers;

        public TimeSpan TotalDuration => TimeSpan.FromMinutes(Customers.Sum(x => x.TotalDuration.TotalMinutes));
    }
}
