using MiddlewareDemoApp.Middlewares; 

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRequestLogging();
app.UseShortCircuit();
app.UseResponseTime();

app.UseStaticFiles();

app.MapGet("/hello", () => "Hello from ASP.NET Core!");

app.Run();
