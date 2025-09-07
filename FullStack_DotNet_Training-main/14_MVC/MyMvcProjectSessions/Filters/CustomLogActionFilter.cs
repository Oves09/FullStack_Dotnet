using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace MyMvcProjectSessions.Filters
{
    public class CustomLogActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Debug.WriteLine("Action Executing: " + context.ActionDescriptor.DisplayName);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Debug.WriteLine("Action Executed: " + context.ActionDescriptor.DisplayName);
        }
    }
}
