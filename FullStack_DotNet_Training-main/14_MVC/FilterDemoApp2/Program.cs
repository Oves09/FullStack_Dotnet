var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<FilterDemoApp2.Filters.CustomAuthFilter>();
    options.Filters.Add<FilterDemoApp2.Filters.CustomResourceFilter>();
    options.Filters.Add<FilterDemoApp2.Filters.LoggingActionFilter>();
    options.Filters.Add<FilterDemoApp2.Filters.CustomExceptionFilter>();
    options.Filters.Add<FilterDemoApp2.Filters.CustomResultFilter>();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


// http://localhost:5190/Home/Index?token=123 => All filters run in order (check console logs).

// https://localhost:5190/Home/Index  => Blocked by Authorization filter (401).

// https://localhost:5190/Home/ThrowError?token=123 => Caught by Exception filter, returns JSON { "Message": "Something went wrong." }.




