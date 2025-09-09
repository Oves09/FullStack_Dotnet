using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace BankingMVC.Filters
{
    public class TransactionLoggingFilter : IActionFilter
    {
        private readonly ILogger<TransactionLoggingFilter> _logger;

        public TransactionLoggingFilter(ILogger<TransactionLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("User {User} performed {Action} at {Time}",
                context.HttpContext.Session.GetString("User"),
                context.ActionDescriptor.DisplayName,
                DateTime.Now);
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
