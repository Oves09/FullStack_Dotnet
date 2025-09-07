using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsByCategoryAsync(string category);
        Task<List<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<List<Product>> FilterProductsAsync(string category, string priceRange);
    }
}