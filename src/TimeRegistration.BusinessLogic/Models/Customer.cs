using System;

namespace TimeRegistration.BusinessLogic.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }

        public string Name { get; set; }

        public Project[] Projects { get; set; }
    }
}
