using Microsoft.AspNetCore.Mvc;

namespace Day_28RoutingDemo.Controllers
{
    public class StoreController : Controller
    {
        // [HttpGet("index")]
        public IActionResult Index()
        {
            ViewData["Message"] = "Welcome to the store!";
            return View();
        }

        [HttpGet("category/{categoryName}")]
        public IActionResult ByCategory(string categoryName)
        {
            ViewData["Category"] = categoryName;
            // Message for category
            ViewData["Message"] = $"Details for category {categoryName} - Attribute routing example";
            return View();
        }

        // product/id:int
        [HttpGet("product/{id:int}")]
        public IActionResult ByProduct(int id)
        {
            ViewData["ProductId"] = id;
            // Message for product
            ViewData["Message"] = $"Details for product {id} - Attribute routing example";
            return View();
        }
    }
}
    