using Microsoft.AspNetCore.Mvc.RazorPages;
using DIServiceDemo.Services;

namespace DIServiceDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMessageService _messageService;

        public string WelcomeMessage { get; set; } = "";

        // Constructor injection
        public IndexModel(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public void OnGet()
        {
            // Use service to get message
            WelcomeMessage = _messageService.GetWelcomeMessage("User");
        }
    }
}
