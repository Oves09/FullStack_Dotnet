using Microsoft.AspNetCore.Mvc;
using MyMvcProjectSessions.Filters;

namespace MyMvcProjectSessions.Controllers
{
    [ServiceFilter(typeof(CustomLogActionFilter))] 
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
        
            Response.Cookies.Append("UserName", "Abhijeet", new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMinutes(10)
            });

           
            var cookieValue = Request.Cookies["UserName"];
            ViewBag.CookieValue = cookieValue;

            return View();
        }

        public IActionResult SetSession()
        {
            // ✅ Set Session
            HttpContext.Session.SetString("SessionUser", "Abhijeet");
            HttpContext.Session.SetInt32("UserId", 101);

            return RedirectToAction("GetSession");
        }

        public IActionResult GetSession()
        {
            // Get Session
            var name = HttpContext.Session.GetString("SessionUser");
            var id = HttpContext.Session.GetInt32("UserId");

            ViewBag.SessionUser = name;
            ViewBag.UserId = id;

            return View();
        }
        public IActionResult ShowSession()
        {
            // Set something in session so it's get tracked
            HttpContext.Session.SetString("SessionUser", "Abhijeet");

            // Get session id
            var sessionId = HttpContext.Session.Id;

            ViewBag.SessionId = sessionId;
            ViewBag.Time = DateTime.Now.ToString("HH:mm:ss");

            return View();
        }

    }
}
