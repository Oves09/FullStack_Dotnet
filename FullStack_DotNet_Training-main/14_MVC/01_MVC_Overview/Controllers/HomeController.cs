using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using _01_MVC_Overview.Models;

namespace _01_MVC_Overview.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewBag.Message = "Welcome to ASP.NET Core MVC !";
        return View();
    }

    public IActionResult About()
    {
        return View();
    }
}
