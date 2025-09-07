using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ECommercePlatform.Services;

namespace ECommercePlatform.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILoggingService _loggingService;

        public GlobalExceptionFilter(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void OnException(ExceptionContext context)
        {
            _loggingService.LogError(context.Exception, "Global exception handler");

            context.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    ["Message"] = "An error occurred while processing your request."
                }
            };

            context.ExceptionHandled = true;
        }
    }
}