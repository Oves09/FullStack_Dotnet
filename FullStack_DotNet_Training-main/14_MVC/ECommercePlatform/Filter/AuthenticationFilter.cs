using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ECommercePlatform.Services;

namespace ECommercePlatform.Filters
{
    public class AuthenticationFilter : IAuthorizationFilter
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationFilter(IAuthenticationService authService)
        {
            _authService = authService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!_authService.IsUserLoggedIn(context.HttpContext))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}