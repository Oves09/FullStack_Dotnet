using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecureTaskManager.Models;
using SecureTaskManager.Services;
using SecureTaskManager.ViewModels;

namespace SecureTaskManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IInputValidationService _validationService;
        private readonly IJwtService _jwtService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IInputValidationService validationService,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _validationService = validationService;
            _jwtService = jwtService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            // Additional input validation
            if (_validationService.ContainsMaliciousContent(model.Email) ||
                _validationService.ContainsMaliciousContent(model.Password))
            {
                ModelState.AddModelError("", "Invalid input detected");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email, 
                model.Password, 
                model.RememberMe, 
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                
                return RedirectToAction("Index", "Tasks");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Account locked due to multiple failed login attempts");
                return View(model);
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Additional input validation
            if (_validationService.ContainsMaliciousContent(model.Email) ||
                _validationService.ContainsMaliciousContent(model.FirstName) ||
                _validationService.ContainsMaliciousContent(model.LastName))
            {
                ModelState.AddModelError("", "Invalid input detected");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = _validationService.EscapeHtml(model.FirstName),
                LastName = _validationService.EscapeHtml(model.LastName)
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Tasks");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // API endpoint for JWT token generation
        [HttpPost]
        [Route("api/auth/token")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return Ok(new
            {
                token,
                expiration = DateTime.UtcNow.AddHours(1),
                user = new
                {
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName
                }
            });
        }
    }
}