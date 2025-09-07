using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureTaskManager.Data;
using SecureTaskManager.Models;

namespace SecureTaskManager.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.CreatedAt,
                    TaskCount = u.Tasks.Count()
                })
                .ToListAsync();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var totalUsers = await _userManager.Users.CountAsync();
            var totalTasks = await _context.Tasks.CountAsync();
            var completedTasks = await _context.Tasks.CountAsync(t => t.IsCompleted);
            var recentUsers = await _userManager.Users
                .OrderByDescending(u => u.CreatedAt)
                .Take(5)
                .Select(u => new { u.FirstName, u.LastName, u.Email, u.CreatedAt })
                .ToListAsync();

            var viewModel = new
            {
                TotalUsers = totalUsers,
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                PendingTasks = totalTasks - completedTasks,
                RecentUsers = recentUsers
            };

            return View(viewModel);
        }
    }
}