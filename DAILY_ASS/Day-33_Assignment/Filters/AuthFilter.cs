using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BankingMVC.Filters
{
    public class AuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(user))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
