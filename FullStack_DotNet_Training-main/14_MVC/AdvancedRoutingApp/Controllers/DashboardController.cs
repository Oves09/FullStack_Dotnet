using Microsoft.AspNetCore.Mvc;

namespace AdvancedRoutingApp.Controllers
{
    public class DashboardController : Controller
    {
        // Dynamic routing based on user role
        [Route("Dashboard")]
        public IActionResult Index()
        {
            // Simulate user role - in real app, this would come from authentication
            var userRole = HttpContext.Session.GetString("UserRole") ?? "User";
            
            if (userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Admin");
            }
            
            return RedirectToAction("User");
        }

        [Route("Dashboard/Admin")]
        public IActionResult Admin()
        {
            ViewBag.Role = "Admin";
            ViewBag.Message = "Welcome to Admin Dashboard";
            return View("Dashboard");
        }

        [Route("Dashboard/User")]
        public IActionResult User()
        {
            ViewBag.Role = "User";
            ViewBag.Message = "Welcome to User Dashboard";
            return View("Dashboard");
        }

        // Action to set user role for testing
        [Route("SetRole/{role}")]
        public IActionResult SetRole(string role)
        {
            HttpContext.Session.SetString("UserRole", role);
            return RedirectToAction("Index");
        }
    }
}