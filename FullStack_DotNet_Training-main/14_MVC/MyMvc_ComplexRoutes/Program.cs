// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddControllersWithViews();

// var app = builder.Build();

// // Middleware
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     app.UseHsts();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();
// app.UseRouting();
// app.UseAuthorization();

// // ---------------- Custom Routing Examples ---------------- //
// app.MapControllerRoute(
//     name: "categoryRoute",
//     pattern: "products/{category}/{id?}",
//     defaults: new { controller = "Products", action = "ByCategory" },
//     constraints: new { id = @"\d+" } // id must be numeric
// );

// app.MapControllerRoute(
//     name: "yearArchive",
//     pattern: "archive/{year:int}/{month:int?}",
//     defaults: new { controller = "Archive", action = "ByDate" }
// );

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

// app.Run();


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

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

app.Run();




// https://localhost:5001/products → Index

// https://localhost:5001/products/details/10 → Details for ID = 10

// https://localhost:5001/products/category/mobile → Category = mobile

// https://localhost:5001/products/category/laptop → Category = laptop

// https://localhost:5001/products/search?keyword=phone&minPrice=20000 → Dynamic Search