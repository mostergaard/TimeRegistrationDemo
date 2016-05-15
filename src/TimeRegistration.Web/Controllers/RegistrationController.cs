using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TimeRegistration.BusinessLogic.Interfaces;
using TimeRegistration.BusinessLogic.Models;
using TimeRegistration.Web.ViewModels.Registration;

namespace TimeRegistration.Web.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IRepository repository;

        public RegistrationController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("/Register")]
        public async Task<IActionResult> Register()
        {
            var customers = await this.repository.GetAllCustomers();

            var model = new RegisterViewModel
            {
                PossibleCustomers = customers,
                CustomerId = customers.First().CustomerId,
                ProjectId = customers.First().Projects.First().ProjectId,
                Date = DateTime.Today,
                Hours = 1,
                Notes = string.Empty
            };

            return View(model);
        }

        [HttpPost("/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                await this.repository.AddRegistration(model.CustomerId, model.ProjectId, new Registration
                {
                    Date = model.Date,
                    Duration = TimeSpan.FromHours(model.Hours),
                    Notes = model.Notes
                });

                return RedirectToAction("MonthOverview", "Main");
            }

            // We have lost the list of possible customers, so rebuild
            model.PossibleCustomers = await this.repository.GetAllCustomers();

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet("/ProjectList")]
        public async Task<IActionResult> ProjectList(Guid customerId)
        {
            var customers = await this.repository.GetAllCustomers();
            var projects = customers.FirstOrDefault(x => x.CustomerId == customerId)?.Projects;

            if (projects != null)
            {
                return Json(projects.Select(x => new { id = x.ProjectId, name = x.Name }).ToList());
            }
            else
            {
                return Json(new object[0]);
            }
        }
    }
}
