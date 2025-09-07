using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Services;

namespace ECommerceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter both username and password.";
                return View();
            }

            var user = await _authService.LoginAsync(username, password);
            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("Username", user.Username);
                
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            var sessionId = HttpContext.Session.Id;
            await _authService.LogoutAsync(sessionId);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
    }
}