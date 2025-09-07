using Microsoft.AspNetCore.Mvc;

namespace MyMvcProjectSessions.Controllers
{
    [Route("products")]   // Base route → /products
    public class ProductsController : Controller
    {
        // GET: /products  or /products/index
        [Route("")]       // Matches /products
        [Route("index")]  // Matches /products/index
        public IActionResult Index()
        {
            return Content("Welcome to the Products Index Page!");
        }

        // GET: /products/details/10
        [Route("details/{id}")]
        public IActionResult Details(int id)
        {
            return Content($"Product Details for ID: {id}");
        }
    }
}
