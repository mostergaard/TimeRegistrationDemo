using System;
using System.Collections.Generic;
using TimeRegistration.BusinessLogic.Models;

namespace TimeRegistration.BusinessLogic.Interfaces
{
    public interface IRepository
    {
        IEnumerable<Customer> GetAllCustomers();

        void AddRegistration(Guid customerId, Guid projectId, Registration registration);
        
        IList<RegistrationWithContext> GetRegistrationsForDateTimeRange(DateTime minDate, DateTime maxDate);

        DateTime? GetEarliestRegistrationDate();

        DateTime? GetLatestRegistrationDate();
    }
}
