using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MiddlewareDemoApp.Middlewares
{
    public class ResponseTimeMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();
            Console.WriteLine($"[TIMING] Response took {stopwatch.ElapsedMilliseconds} ms");
        }
    }

    public static class ResponseTimeMiddlewareExtensions
    {
        public static IApplicationBuilder UseResponseTime(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseTimeMiddleware>();
        }
    }
}
