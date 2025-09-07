using ECommerceApp.Models;
using ECommerceApp.Models.ViewModels;
using ECommerceApp.Services;

namespace ECommerceApp.Services
{
    public class CartService : ICartService
    {
        private static readonly List<CartItem> CartItems = new();
        private readonly IProductService _productService;

        public CartService(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<CartViewModel> GetCartAsync(string sessionId)
        {
            var items = CartItems.Where(c => c.SessionId == sessionId).ToList();
            
            foreach (var item in items)
            {
                item.Product = await _productService.GetProductByIdAsync(item.ProductId);
            }

            return new CartViewModel { Items = items };
        }

        public async Task AddToCartAsync(string sessionId, int productId, int quantity = 1)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null) return;

            var existingItem = CartItems.FirstOrDefault(c => 
                c.SessionId == sessionId && c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                CartItems.Add(new CartItem
                {
                    Id = CartItems.Count + 1,
                    ProductId = productId,
                    Quantity = quantity,
                    SessionId = sessionId,
                    AddedDate = DateTime.Now
                });
            }
        }

        public Task RemoveFromCartAsync(string sessionId, int productId)
        {
            var item = CartItems.FirstOrDefault(c => 
                c.SessionId == sessionId && c.ProductId == productId);
            if (item != null)
            {
                CartItems.Remove(item);
            }
            return Task.CompletedTask;
        }

        public Task UpdateQuantityAsync(string sessionId, int productId, int quantity)
        {
            var item = CartItems.FirstOrDefault(c => 
                c.SessionId == sessionId && c.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    CartItems.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
            }
            return Task.CompletedTask;
        }

        public Task ClearCartAsync(string sessionId)
        {
            CartItems.RemoveAll(c => c.SessionId == sessionId);
            return Task.CompletedTask;
        }
    }
}