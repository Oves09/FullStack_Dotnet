using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Build app
var app = builder.Build();

// Error handling + HSTS in Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// Force HTTPS
app.UseHttpsRedirection();

// Simple request/response logging middleware
app.Use(async (context, next) =>
{
    var logger = app.Logger;
    logger.LogInformation("Request: {Method} {Path}", context.Request.Method, context.Request.Path);
    await next();
    logger.LogInformation("Response: {StatusCode}", context.Response.StatusCode);
});

// Content Security Policy (basic)
app.Use(async (context, next) =>
{
    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; script-src 'self'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; object-src 'none'; upgrade-insecure-requests";
    await next();
});

// Serve static files with caching
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // 7 days cache
        ctx.Context.Response.Headers["Cache-Control"] = "public,max-age=604800";
    }
});

// Simple error endpoint (custom page)
app.MapGet("/error", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(@"
        <html><head><title>Error</title></head>
        <body><h1>Oops! Something went wrong.</h1>
        <p>Custom error page.</p></body></html>");
});

// Redirect root to our static index
app.MapGet("/", context =>
{
    context.Response.Redirect("/index.html");
    return Task.CompletedTask;
});

app.Run();
