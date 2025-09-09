using Day26_assignment2_razorPages.Services; // make sure namespace matches

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register ItemService in DI container
builder.Services.AddSingleton<ItemService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // important for css/js

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
