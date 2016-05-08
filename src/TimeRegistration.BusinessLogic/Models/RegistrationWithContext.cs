using System;

namespace TimeRegistration.BusinessLogic.Models
{
    public class RegistrationWithContext
    {
        public Guid CustomerId { get; set; }

        public Guid ProjectId { get; set; }

        public Registration Registration { get; set; }
    }
}
