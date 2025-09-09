using Microsoft.AspNetCore.Mvc;
using FeedbackPortal.Models;

namespace FeedbackPortal.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(UserRegistration user)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Success");
            }
            return View(user);
        }

        public IActionResult Success() => View();
    }
}
