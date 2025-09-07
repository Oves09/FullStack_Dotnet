//Resource Filter

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FilterDemoApp2.Filters
{
    public class CustomResourceFilter : IResourceFilter
    {
        private readonly ILogger<CustomResourceFilter> _logger;
        public CustomResourceFilter(ILogger<CustomResourceFilter> logger)
        {
            _logger = logger;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _logger.LogInformation("Resource filter: Before controller execution");
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            _logger.LogInformation("Resource filter: After controller execution");
        }
    }
}
