using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommercePlatform.Data;
using ECommercePlatform.Models;
using ECommercePlatform.Filters;
using ECommercePlatform.Services;

namespace ECommercePlatform.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthenticationService _authService;

        public OrdersController(ApplicationDbContext context, IAuthenticationService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = _authService.GetCurrentUser(HttpContext);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            var orders = await _context.Orders
                .Where(o => o.UserId == currentUser.Id)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}