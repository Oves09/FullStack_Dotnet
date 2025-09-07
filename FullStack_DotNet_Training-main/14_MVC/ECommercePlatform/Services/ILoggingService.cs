namespace ECommercePlatform.Services
{
    public interface ILoggingService
    {
        void LogRequest(string method, string url, int statusCode, long responseTime);
        void LogError(Exception exception, string context);
        void LogInformation(string message);
    }
}