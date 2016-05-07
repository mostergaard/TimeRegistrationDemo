using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TimeRegistration.Web.Models;
using TimeRegistration.Web.ViewModels.Main;

namespace TimeRegistration.Web.Controllers
{
    public class MainController : Controller
    {
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
            var customers = new[]
            {
                new Customer
                {
                    Name = "Customer A",
                    Projects = new[] { "Project 1", "Project 2" }
                },
                new Customer
                {
                    Name = "Customer B A/S",
                    Projects = new[] { "Project x", "Project y" }
                },
            };

            var model = new RegisterViewModel
            {
                CustomersAndProjects = customers,
                AllCustomers = customers.Select(x => x.Name).ToList(),
                AllProjects = customers.SelectMany(x => x.Projects).ToList(),
                Customer = customers.First().Name,
                Project = customers.First().Projects.First(),
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
                return RedirectToAction("MonthOverview");
            }

            // If we got this far, something failed, redisplay form
            return View(model);

            // TODO: Need to set the Customers etc. properties on the Model for the view to load!
        }
    }
}
