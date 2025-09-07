
using DIApp.Services;

var builder = WebApplication.CreateBuilder(args);

//Registering Services
builder.Services.AddTransient<IMessageService, EmailMessageService>();

var app = builder.Build();

app.MapGet("/", (IMessageService messageService) =>
{
    //ASP.NET Core automatically injects the IMessageService
    return messageService.GetMessage();
});

app.Run();
