using Microsoft.AspNetCore.Mvc;

namespace Day_28RoutingDemo.Controllers;

public class ProductsController : Controller
{
    public IActionResult Index()
    {
        ViewData["Message"] = "List of products.";
        return View();
    }

    public IActionResult Details(int id)
    {
        ViewData["ProductId"] = id;
        ViewData["Message"] = $"Product details for product {id}.";
        return View();
    }
}
