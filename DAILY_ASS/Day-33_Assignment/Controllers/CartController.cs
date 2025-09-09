using Microsoft.AspNetCore.Mvc;

namespace EcommerceRoutingDemo.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Checkout()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return Content("Welcome to Checkout - You are logged in!");
        }
    }
}
