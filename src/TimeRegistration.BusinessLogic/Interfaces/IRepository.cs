using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeRegistration.BusinessLogic.Models;

namespace TimeRegistration.BusinessLogic.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomers();

        Task AddRegistration(Guid customerId, Guid projectId, Registration registration);

        Task<IList<RegistrationWithContext>> GetRegistrationsForDateTimeRange(DateTime minDate, DateTime maxDate);

        Task<DateTime?> GetEarliestRegistrationDate();

        Task<DateTime?> GetLatestRegistrationDate();
    }
}
