using AdvancedRoutingApp.Constraints;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Configure custom route constraints
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}",
    constraints: new { id = new GuidRouteConstraint() }
);

// Add custom route constraints to the route options
var routeOptions = app.Services.GetRequiredService<IOptions<RouteOptions>>();
routeOptions.Value.ConstraintMap["guid"] = typeof(GuidRouteConstraint);
routeOptions.Value.ConstraintMap["category"] = typeof(CategoryConstraint);

app.Run();

// Make the Program class public for testing
public partial class Program { }