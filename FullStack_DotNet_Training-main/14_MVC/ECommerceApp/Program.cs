// using ECommerceApp.Services;
// using ECommerceApp.Constraints;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddControllersWithViews();
// builder.Services.AddSession(options =>
// {
//     options.IdleTimeout = TimeSpan.FromMinutes(30);
//     options.Cookie.HttpOnly = true;
//     options.Cookie.IsEssential = true;
// });

// // Register application services
// builder.Services.AddScoped<IProductService, ProductService>();
// builder.Services.AddScoped<ICartService, CartService>();
// builder.Services.AddScoped<IAuthService, AuthService>();

// // Configure route options
// builder.Services.Configure<RouteOptions>(options =>
// {
//     options.ConstraintMap.Add("category", typeof(CategoryConstraint));
//     options.ConstraintMap.Add("pricerange", typeof(PriceRangeConstraint));
// });

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     app.UseHsts();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();
// app.UseSession();
// app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

// app.Run();

// // Make the Program class public for testing
// public partial class Program { }



using ECommerceApp.Services;
using ECommerceApp.Constraints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register application services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configure authentication (Cookie-based)
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";  // Redirect to login when unauthorized
        options.AccessDeniedPath = "/Account/AccessDenied"; // Optional - access denied page
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Optional - cookie expiry
    });

builder.Services.AddAuthorization();

// Configure route options
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("category", typeof(CategoryConstraint));
    options.ConstraintMap.Add("pricerange", typeof(PriceRangeConstraint));
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

// Order matters: Authentication -> Session -> Authorization
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Make the Program class public for testing
public partial class Program { }
