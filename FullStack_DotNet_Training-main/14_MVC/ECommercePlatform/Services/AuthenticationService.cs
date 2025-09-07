using ECommercePlatform.Data;
using ECommercePlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;

        public AuthenticationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public bool IsUserLoggedIn(HttpContext context)
        {
            return context.Session.GetString("UserId") != null;
        }

        public User? GetCurrentUser(HttpContext context)
        {
            var userIdString = context.Session.GetString("UserId");
            if (int.TryParse(userIdString, out int userId))
            {
                return _context.Users.Find(userId);
            }
            return null;
        }
    }
}