using ECommercePlatform.Models;

namespace ECommercePlatform.Services
{
    public interface IAuthenticationService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        bool IsUserLoggedIn(HttpContext context);
        User? GetCurrentUser(HttpContext context);
    }
}