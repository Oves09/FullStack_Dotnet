using Microsoft.Extensions.Logging;

namespace ECommercePlatform.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void LogRequest(string method, string url, int statusCode, long responseTime)
        {
            _logger.LogInformation("Request: {Method} {Url} - Status: {StatusCode} - Duration: {ResponseTime}ms",
                method, url, statusCode, responseTime);
        }

        public void LogError(Exception exception, string context)
        {
            _logger.LogError(exception, "Error in {Context}: {Message}", context, exception.Message);
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }
}