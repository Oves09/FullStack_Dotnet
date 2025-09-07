using Microsoft.AspNetCore.Mvc;
using ECommercePlatform.Models;
using ECommercePlatform.Services;


namespace ECommercePlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly ILoggingService _loggingService;

        public AccountController(IAuthenticationService authService, ILoggingService loggingService)
        {
            _authService = authService;
            _loggingService = loggingService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.AuthenticateAsync(model.Username, model.Password);
                if (user != null)
                {
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);
                    
                    _loggingService.LogInformation($"User {user.Username} logged in successfully");
                    return RedirectToAction("Index", "Home");
                }
                
                ModelState.AddModelError("", "Invalid username or password");
            }
            
            return View(model);
        }

        public IActionResult Logout()
        {
            var username = HttpContext.Session.GetString("Username");
            HttpContext.Session.Clear();
            _loggingService.LogInformation($"User {username} logged out");
            return RedirectToAction("Index", "Home");
        }
    }
}