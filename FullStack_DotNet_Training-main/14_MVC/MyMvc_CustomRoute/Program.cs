using MyMvc_CustomRoute.Models; // add namespace

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// ✅ Register custom constraint
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("even", typeof(EvenNumberConstraint));
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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ✅ Custom Route with Even Constraint
app.MapControllerRoute(
    name: "EvenRoute",
    pattern: "products/{id:even}",
    defaults: new { controller = "Products", action = "Details" });

app.Run();
