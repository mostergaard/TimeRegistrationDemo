using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TimeRegistration.BusinessLogic.Interfaces;
using TimeRegistration.BusinessLogic.Models;
using TimeRegistration.Web.ViewModels.Main;

namespace TimeRegistration.Web.Controllers
{
    public class MainController : Controller
    {
        private IRepository repository;

        public MainController(IRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Default()
        {
            return RedirectToAction("MonthOverview");
        }

        public IActionResult MonthOverview()
        {
            return View();
        }

        public IActionResult Register()
        {
            var customers = this.repository.GetAllCustomers().ToArray();

            var model = new RegisterViewModel
            {
                CustomersAndProjects = customers,
                AllCustomers = customers,
                AllProjects = customers.SelectMany(x => x.Projects).ToList(),
                CustomerId = customers.First().CustomerId,
                ProjectId = customers.First().Projects.First().ProjectId,
                Date = DateTime.Today,
                Hours = 1,
                Notes = string.Empty
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                this.repository.AddRegistration(model.CustomerId, model.ProjectId, new Registration
                {
                    Date = model.Date,
                    Duration = TimeSpan.FromHours(model.Hours),
                    Notes = model.Notes
                });

                return RedirectToAction("MonthOverview");
            }

            // We have lost the list of possible selections, so rebuild
            var customers = this.repository.GetAllCustomers().ToArray();
            model.CustomersAndProjects = customers;
            model.AllCustomers = customers;
            model.AllProjects = customers.SelectMany(x => x.Projects).ToList();

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
