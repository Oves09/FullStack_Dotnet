using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string username, string password);
        Task LogoutAsync(string sessionId);
        Task<bool> IsUserLoggedInAsync(string sessionId);
        Task<User?> GetCurrentUserAsync(string sessionId);
    }
}