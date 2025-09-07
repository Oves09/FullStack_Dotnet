using SecureTaskManager.Models;

namespace SecureTaskManager.Services
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
        bool ValidateToken(string token);
    }
}