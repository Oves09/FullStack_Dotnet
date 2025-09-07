using Microsoft.AspNetCore.Mvc;
using AdvancedRoutingApp.Models;

namespace AdvancedRoutingApp.Controllers
{
    public class UsersController : Controller
    {
        private static readonly List<User> Users = new()
        {
            new User 
            { 
                Id = 1, 
                Username = "john_doe", 
                Email = "john@example.com", 
                Role = "Admin",
                Orders = new List<Order>
                {
                    new Order { Id = 1, ProductName = "Laptop", Amount = 999.99m, OrderDate = DateTime.Now.AddDays(-5) },
                    new Order { Id = 2, ProductName = "Mouse", Amount = 29.99m, OrderDate = DateTime.Now.AddDays(-2) }
                }
            },
            new User 
            { 
                Id = 2, 
                Username = "jane_smith", 
                Email = "jane@example.com", 
                Role = "User",
                Orders = new List<Order>
                {
                    new Order { Id = 3, ProductName = "Book", Amount = 19.99m, OrderDate = DateTime.Now.AddDays(-1) }
                }
            }
        };

        // Complex route: /Users/{username}/Orders
        [Route("Users/{username}/Orders")]
        public IActionResult Orders(string username)
        {
            var user = Users.FirstOrDefault(u => 
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                return NotFound($"User '{username}' not found");
            }

            ViewBag.Username = username;
            return View(user.Orders);
        }

        [Route("Users/{username}")]
        public IActionResult Profile(string username)
        {
            var user = Users.FirstOrDefault(u => 
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Index()
        {
            return View(Users);
        }
    }
}