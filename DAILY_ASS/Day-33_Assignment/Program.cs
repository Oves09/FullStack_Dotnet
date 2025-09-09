using BankingMVC.Services;
using BankingMVC.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<TransactionLoggingFilter>();
    options.Filters.Add<ErrorHandlingFilter>();
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<IAuthService, AuthService>();

// 🔹 Register filters so ServiceFilter / TypeFilter can resolve them
builder.Services.AddScoped<AuthFilter>();
builder.Services.AddScoped<RoleAuthFilter>(_ => new RoleAuthFilter("Admin"));
builder.Services.AddScoped<TransactionLoggingFilter>();
builder.Services.AddScoped<ErrorHandlingFilter>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.MapDefaultControllerRoute();
app.Run();
