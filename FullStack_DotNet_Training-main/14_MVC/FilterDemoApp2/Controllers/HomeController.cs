using Microsoft.AspNetCore.Mvc;

namespace FilterDemoApp2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Hello! All filters executed successfully.");
        }

        public IActionResult ThrowError()
        {
            throw new Exception("Simulated error to test Exception Filter.");
        }
    }
}
