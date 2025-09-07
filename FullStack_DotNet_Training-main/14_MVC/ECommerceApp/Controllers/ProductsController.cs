using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Services;
using ECommerceApp.Models;

namespace ECommerceApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public ProductsController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        // Complex route: /Products/{category}/{id}
        [Route("Products/{category:category}/{id:int}")]
        public async Task<IActionResult> Details(string category, int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            
            if (product == null || !product.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            {
                return NotFound($"Product with ID {id} not found in category '{category}'");
            }

            ViewBag.Category = category;
            return View(product);
        }

        // Category listing with constraint
        [Route("Products/Category/{category:category}")]
        public async Task<IActionResult> Category(string category)
        {
            var products = await _productService.GetProductsByCategoryAsync(category);
            ViewBag.Category = category;
            return View(products);
        }

        // Custom constraint for product filtering
        [Route("Products/Filter/{category:category}/{priceRange:pricerange}")]
        public async Task<IActionResult> Filter(string category, string priceRange)
        {
            var products = await _productService.FilterProductsAsync(category, priceRange);
            ViewBag.Category = category;
            ViewBag.PriceRange = priceRange;
            return View("FilterResults", products);
        }

        // Add to cart
        [HttpPost]
        [Route("Products/AddToCart")]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var sessionId = HttpContext.Session.Id;
            await _cartService.AddToCartAsync(sessionId, productId, quantity);
            
            TempData["Message"] = "Product added to cart successfully!";
            return RedirectToAction("Index");
        }
    }
}