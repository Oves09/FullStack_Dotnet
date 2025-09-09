using Microsoft.AspNetCore.Mvc;

[Route("blog")]
public class BlogController : Controller
{
    [HttpGet("{year:int:min(2020)}/{month:int:range(1,12)}/{day:int:range(1,31)?}")]
    public IActionResult Archive(int year, int month, int? day)
    {
        ViewData["Year"] = year;
        ViewData["Month"] = month;
        ViewData["Day"] = day;
        return View();
    }
}