using Microsoft.AspNetCore.Mvc;
using BankingMVC.Services;

namespace BankingMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (_authService.Login(username, password))
            {
                HttpContext.Session.SetString("User", username);
                HttpContext.Session.SetString("Role", _authService.GetRole(username));
                return RedirectToAction("Dashboard", "Transaction");
            }
            ViewBag.Error = "Invalid credentials!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();
    }
}
