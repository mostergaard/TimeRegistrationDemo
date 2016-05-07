using Microsoft.AspNet.Mvc;

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
            return View();
        }
    }
}
