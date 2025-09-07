using Microsoft.AspNetCore.Mvc;

namespace MyMvc_ComplexRoutes.Controllers
{
    public class ProductsController : Controller
    {
        // Default action
        public IActionResult Index()
        {
            return Content("Welcome to Products Index!");
        }

        // Route with parameter
        [Route("products/details/{id:int}")]
        public IActionResult Details(int id)
        {
            return Content($"Product Details for ID = {id}");
        }

        // Route with custom constraint (only 'mobile' or 'laptop')
        [Route("products/category/{category:regex(^mobile|laptop$)}")]
        public IActionResult Category(string category)
        {
            return Content($"Products in Category: {category}");
        }

        // Dynamic routing based on query string
        [Route("products/search")]
        public IActionResult Search(string keyword, int? minPrice)
        {
            return Content($"Searching for {keyword}, Min Price: {minPrice}");
        }
    }
}
