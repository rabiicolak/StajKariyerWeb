using Microsoft.AspNetCore.Mvc;

namespace StajKariyerWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}