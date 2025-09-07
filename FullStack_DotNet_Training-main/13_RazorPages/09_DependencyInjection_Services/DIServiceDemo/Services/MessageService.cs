namespace DIServiceDemo.Services
{
    // Implementation of IMessageService
    public class MessageService : IMessageService
    {
        public string GetWelcomeMessage(string name)
        {
            return $"Hello, {name}! Welcome to Razor Pages with DI.";
        }
    }
}
