using ECommerceApp.Models;
using ECommerceApp.Services;

namespace ECommerceApp.Services
{
    public class ProductService : IProductService
    {
        private static readonly List<Product> Products = new()
        {
            new Product { Id = 1, Name = "Gaming Laptop", Category = "electronics", Price = 1299.99m, Description = "High-performance gaming laptop", StockQuantity = 10, ImageUrl = "/images/laptop.jpg" },
            new Product { Id = 2, Name = "Wireless Headphones", Category = "electronics", Price = 199.99m, Description = "Premium wireless headphones", StockQuantity = 25, ImageUrl = "/images/headphones.jpg" },
            new Product { Id = 3, Name = "Smart Watch", Category = "electronics", Price = 299.99m, Description = "Feature-rich smartwatch", StockQuantity = 15, ImageUrl = "/images/smartwatch.jpg" },
            new Product { Id = 4, Name = "Running Shoes", Category = "clothing", Price = 89.99m, Description = "Comfortable running shoes", StockQuantity = 30, ImageUrl = "/images/shoes.jpg" },
            new Product { Id = 5, Name = "T-Shirt", Category = "clothing", Price = 24.99m, Description = "Premium cotton t-shirt", StockQuantity = 50, ImageUrl = "/images/tshirt.jpg" },
            new Product { Id = 6, Name = "Programming Guide", Category = "books", Price = 49.99m, Description = "Complete programming guide", StockQuantity = 20, ImageUrl = "/images/book.jpg" },
            new Product { Id = 7, Name = "Coffee Maker", Category = "home", Price = 79.99m, Description = "Automatic coffee maker", StockQuantity = 12, ImageUrl = "/images/coffee.jpg" },
            new Product { Id = 8, Name = "Tennis Racket", Category = "sports", Price = 129.99m, Description = "Professional tennis racket", StockQuantity = 8, ImageUrl = "/images/racket.jpg" }
        };

        public Task<List<Product>> GetAllProductsAsync()
        {
            return Task.FromResult(Products);
        }

        public Task<Product?> GetProductByIdAsync(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);
        }

        public Task<List<Product>> GetProductsByCategoryAsync(string category)
        {
            var products = Products.Where(p => 
                p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            return Task.FromResult(products);
        }

        public Task<List<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var products = Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
            return Task.FromResult(products);
        }

        public Task<List<Product>> FilterProductsAsync(string category, string priceRange)
        {
            var parts = priceRange.Split('-');
            if (parts.Length == 2 && decimal.TryParse(parts[0], out var min) && decimal.TryParse(parts[1], out var max))
            {
                var products = Products.Where(p => 
                    p.Category.Equals(category, StringComparison.OrdinalIgnoreCase) && 
                    p.Price >= min && p.Price <= max).ToList();
                return Task.FromResult(products);
            }
            return Task.FromResult(new List<Product>());
        }
    }
}