using ECommerceApp.Models;
using ECommerceApp.Models.ViewModels;

namespace ECommerceApp.Services
{
    public interface ICartService
    {
        Task<CartViewModel> GetCartAsync(string sessionId);
        Task AddToCartAsync(string sessionId, int productId, int quantity = 1);
        Task RemoveFromCartAsync(string sessionId, int productId);
        Task UpdateQuantityAsync(string sessionId, int productId, int quantity);
        Task ClearCartAsync(string sessionId);
    }
}