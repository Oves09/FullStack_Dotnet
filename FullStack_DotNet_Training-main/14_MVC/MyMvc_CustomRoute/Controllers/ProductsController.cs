using Microsoft.AspNetCore.Mvc;

namespace MyMvc_CustomRoute.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Details(int id)
        {
            return Content($"Product Details for ID: {id}");
        }
    }
}
