using Microsoft.AspNetCore.Mvc.Filters;
using ECommercePlatform.Services;
using System.Diagnostics;

namespace ECommercePlatform.Filters
{
    public class LoggingFilter : IAsyncActionFilter
    {
        private readonly ILoggingService _loggingService;

        public LoggingFilter(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.HttpContext.Request;
            
            _loggingService.LogInformation($"Executing action: {context.ActionDescriptor.DisplayName}");
            
            var result = await next();
            
            stopwatch.Stop();
            
            _loggingService.LogRequest(
                request.Method,
                request.Path,
                context.HttpContext.Response.StatusCode,
                stopwatch.ElapsedMilliseconds
            );

            if (result.Exception != null)
            {
                _loggingService.LogError(result.Exception, "Action execution");
            }
        }
    }
}