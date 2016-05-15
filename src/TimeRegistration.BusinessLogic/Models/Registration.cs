using System;

namespace TimeRegistration.BusinessLogic.Models
{
    public class Registration
    {
        private readonly DateTime date;
        private readonly TimeSpan duration;
        private readonly string notes;

        public Registration(DateTime date, TimeSpan duration, string notes)
        {
            this.date = date;
            this.duration = duration;
            this.notes = notes;
        }

        public DateTime Date => this.date;

        public TimeSpan Duration => this.duration;

        public string Notes => this.notes;
    }
}
