// Result FIlter

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FilterDemoApp2.Filters
{
    public class CustomResultFilter : IResultFilter
    {
        private readonly ILogger<CustomResultFilter> _logger;
        public CustomResultFilter(ILogger<CustomResultFilter> logger)
        {
            _logger = logger;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            _logger.LogInformation("Result filter: Before result execution.");
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            _logger.LogInformation("Result filter: After result execution.");
        }
    }
}
