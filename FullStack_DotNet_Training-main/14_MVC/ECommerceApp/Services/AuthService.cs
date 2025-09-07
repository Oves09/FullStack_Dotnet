using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public class AuthService : IAuthService
    {
        private static readonly List<User> Users = new()
        {
            new User { Id = 1, Username = "admin", Email = "admin@example.com", Password = "admin123" },
            new User { Id = 2, Username = "john", Email = "john@example.com", Password = "password123" },
            new User { Id = 3, Username = "jane", Email = "jane@example.com", Password = "password456" }
        };

        private static readonly Dictionary<string, User> LoggedInUsers = new();

        public Task<User?> LoginAsync(string username, string password)
        {
            var user = Users.FirstOrDefault(u => 
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == password);
            
            return Task.FromResult(user);
        }

        public Task LogoutAsync(string sessionId)
        {
            LoggedInUsers.Remove(sessionId);
            return Task.CompletedTask;
        }

        public Task<bool> IsUserLoggedInAsync(string sessionId)
        {
            return Task.FromResult(LoggedInUsers.ContainsKey(sessionId));
        }

        public Task<User?> GetCurrentUserAsync(string sessionId)
        {
            LoggedInUsers.TryGetValue(sessionId, out var user);
            return Task.FromResult(user);
        }
    }
}