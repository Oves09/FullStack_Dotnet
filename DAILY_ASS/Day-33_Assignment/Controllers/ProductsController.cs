using Microsoft.AspNetCore.Mvc;

namespace EcommerceRoutingDemo.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Details(string category, int id)
        {
            return Content($"Product in Category: {category}, ID: {id}");
        }

        public IActionResult Filter(string category, string priceRange)
        {
            return Content($"Filtered Products in {category} within {priceRange}");
        }
    }
}
