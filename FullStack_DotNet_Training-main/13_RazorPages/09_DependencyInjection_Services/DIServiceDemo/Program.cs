var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register custom service for DI
builder.Services.AddSingleton<DIServiceDemo.Services.IMessageService, DIServiceDemo.Services.MessageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.Run();
