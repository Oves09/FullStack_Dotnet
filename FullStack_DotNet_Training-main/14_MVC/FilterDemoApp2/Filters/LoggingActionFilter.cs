//Action Filter

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FilterDemoApp2.Filters
{
    public class LoggingActionFilter : IActionFilter
    {
        private readonly ILogger<LoggingActionFilter> _logger;
        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"[ActionFilter] Action {context.ActionDescriptor.DisplayName} started.");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"[ActionFilter] Action {context.ActionDescriptor.DisplayName} finished.");
        }
    }
}
