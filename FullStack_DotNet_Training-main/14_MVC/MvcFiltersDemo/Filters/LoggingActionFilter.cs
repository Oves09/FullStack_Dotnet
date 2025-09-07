using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MvcFiltersDemo.Filters
{
    public class LoggingActionFilter : IActionFilter
    {
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        // Runs before the action executes
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"[Before Action] Controller: {context.Controller}, Action: {context.ActionDescriptor.DisplayName}");
        }

        // Runs after the action executes
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"[After Action] Action executed: {context.ActionDescriptor.DisplayName}");
        }
    }
}
