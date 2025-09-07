using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthApp.Models;

namespace SimpleAuthApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Welcome()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.FullName = $"{user?.FirstName} {user?.LastName}";
            return View();
        }
    }
}
