using Microsoft.AspNetCore.Mvc;
 namespace RoutingDemo.Controllers
 {
     [Route("users")]
     public class UserController : Controller
     {
         [HttpGet("")]
         public IActionResult Index()
         {
             ViewData["Message"] = "User Management - Home Page";
             return View();
         }
         [HttpGet("profile/{username}")]
         public IActionResult Profile(string username, string tab = "overview")
         {
             ViewData["Username"] = username;
             ViewData["Tab"] = tab;
             ViewData["Message"] = $"Profile page for user: {username} - Tab: {tab}";
             return View();
         }
        
     }
 }