using Microsoft.AspNetCore.Mvc;
using BankingMVC.Filters;

namespace BankingMVC.Controllers
{
    [ServiceFilter(typeof(RoleAuthFilter))]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
