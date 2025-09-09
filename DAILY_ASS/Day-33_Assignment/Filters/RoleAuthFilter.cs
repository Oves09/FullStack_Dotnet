using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BankingMVC.Filters
{
    public class RoleAuthFilter : IAuthorizationFilter
    {
        private readonly string _role;

        public RoleAuthFilter(string role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userRole = context.HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(userRole) || userRole != _role)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }
        }
    }
}
