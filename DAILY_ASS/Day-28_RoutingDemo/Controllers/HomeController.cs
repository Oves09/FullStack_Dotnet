using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Day_28RoutingDemo.Models;

namespace Day_28RoutingDemo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult About()
    {
        ViewData["Message"] = "Your application description page.";
        // View data is used for passing data from the controller to the view
        //You can also use ViewBag, which is a dynamic wrapper around ViewData
        return View();
    }

    public IActionResult Contact(string department)
    {
        // department is an optional parameter
        ViewData["Department"] = department;
        ViewData["Message"] = $"Your contact page for {department}.";
        return View();
    }
}
