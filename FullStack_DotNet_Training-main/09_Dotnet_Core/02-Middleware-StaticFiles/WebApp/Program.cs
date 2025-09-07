using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Redirect HTTP → HTTPS
app.UseHttpsRedirection();

// Add custom security headers
app.UseMiddleware<SecurityHeadersMiddleware>();

// Configure static file options
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".webmanifest"] = "application/manifest+json";

app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider,
    ServeUnknownFileTypes = false,
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers["Cache-Control"] = "public,max-age=604800";
        ctx.Context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    }
});

// Demo endpoint
app.MapGet("/hello", () => new { Message = "Hello from middleware pipeline!" });

app.Run();
