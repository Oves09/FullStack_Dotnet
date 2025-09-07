using Microsoft.AspNetCore.Mvc;
using AdvancedRoutingApp.Models;

namespace AdvancedRoutingApp.Controllers
{
    public class ProductsController : Controller
    {
        private static readonly List<Product> Products = new()
        {
            new Product { Id = 1, Name = "Laptop", Category = "electronics", Price = 999.99m, Description = "High-performance laptop" },
            new Product { Id = 2, Name = "T-Shirt", Category = "clothing", Price = 29.99m, Description = "Cotton t-shirt" },
            new Product { Id = 3, Name = "Programming Book", Category = "books", Price = 49.99m, Description = "Learn programming" },
            new Product { Id = 4, Name = "Coffee Mug", Category = "home", Price = 15.99m, Description = "Ceramic coffee mug" },
            new Product { Id = 5, Name = "Tennis Racket", Category = "sports", Price = 129.99m, Description = "Professional tennis racket" }
        };

        // Complex route: /Products/{category}/{id}
        [Route("Products/{category}/{id:int}")]
        public IActionResult Details(string category, int id)
        {
            var product = Products.FirstOrDefault(p => 
                p.Id == id && p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Category = category;
            return View(product);
        }

        // Route with custom constraint
        [Route("Products/ById/{id:guid}")]
        public IActionResult GetByGuid(Guid id)
        {
            ViewBag.SearchedId = id;
            return View("GuidSearch");
        }

        // Category listing with constraint
        [Route("Products/Category/{category:category}")]
        public IActionResult Category(string category)
        {
            var products = Products.Where(p => 
                p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

            ViewBag.Category = category;
            return View(products);
        }

        public IActionResult Index()
        {
            return View(Products);
        }
    }
}