using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeRegistration.BusinessLogic.Models;

namespace TimeRegistration.BusinessLogic.Interfaces
{
    public interface IRepository
    {
        Task<Customer[]> GetAllCustomers();

        Task AddRegistration(Guid customerId, Guid projectId, Registration registration);

        Task<RegistrationWithContext[]> GetRegistrationsForDateRange(DateTime minDate, DateTime maxDate);

        Task<DateTime?> GetEarliestRegistrationDate();

        Task<DateTime?> GetLatestRegistrationDate();
    }
}
