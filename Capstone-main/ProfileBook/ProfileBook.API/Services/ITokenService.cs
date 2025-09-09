using ProfileBook.API.Models;

namespace ProfileBook.API.Services
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
