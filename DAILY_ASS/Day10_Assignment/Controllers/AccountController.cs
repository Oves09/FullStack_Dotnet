using Microsoft.AspNetCore.Mvc;

using Day10_Assignment.Models;


namespace Day10_Assignment.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            if (UserService.Register(username, password))
                return RedirectToAction("Login");

            ViewBag.Message = "User already exists.";
            return View();
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (UserService.Authenticate(username, password))
                return RedirectToAction("Success");

            ViewBag.Message = "Invalid credentials.";
            return View();
        }

        public IActionResult Success() => View();
    }
}
