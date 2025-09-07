using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MiddlewareDemoApp.Middlewares
{
    public class ShortCircuitMiddleware
    {
        private readonly RequestDelegate _next;

        public ShortCircuitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/stop"))
            {
                await context.Response.WriteAsync("Pipeline stopped by ShortCircuitMiddleware!");
                return; // stops further middleware
            }
            await _next(context);
        }
    }

    public static class ShortCircuitMiddlewareExtensions
    {
        public static IApplicationBuilder UseShortCircuit(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ShortCircuitMiddleware>();
        }
    }
}
