using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FilterDemoApp2.Filters
{
    public class CustomAuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Example: Simple check for query param token
            if (!context.HttpContext.Request.Query.ContainsKey("token"))
            {
                context.Result = new UnauthorizedResult(); // 401
            }
        }
    }
}
