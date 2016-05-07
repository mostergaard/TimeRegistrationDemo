using System;

namespace TimeRegistration.BusinessLogic.Models
{
    public class Registration
    {
        public DateTime Date { get; set; }

        public TimeSpan Duration { get; set; }

        public string Notes { get; set; }
    }
}
