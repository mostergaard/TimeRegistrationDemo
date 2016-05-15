using System;

namespace TimeRegistration.BusinessLogic.Models
{
    public class RegistrationWithContext
    {
        private readonly Guid customerId;
        private readonly Guid projectId;
        private readonly Registration registration;

        public RegistrationWithContext(Guid customerId, Guid projectId, Registration registration)
        {
            this.customerId = customerId;
            this.projectId = projectId;
            this.registration = registration;
        }

        public Guid CustomerId => this.customerId;

        public Guid ProjectId => this.projectId;

        public Registration Registration => this.registration;
    }
}
