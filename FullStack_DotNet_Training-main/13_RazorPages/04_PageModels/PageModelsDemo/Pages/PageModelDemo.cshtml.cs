using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PageModelsDemo.Pages
{
    public class PageModelDemoModel : PageModel
    {
        public string Message { get; set; } = "Welcome!";

        public void OnGet()
        {
            Message = "OnGet() called – Page loaded.";
        }

        public void OnPost()
        {
            Message = "OnPost() called – Default handler.";
        }

        public async Task OnPostSaveAsync()
        {
            await Task.Delay(500); // Simulating async work
            Message = "OnPostSaveAsync() called – Data saved.";
        }

        public void OnPostDelete()
        {
            Message = "OnPostDelete() called – Data deleted.";
        }
    }
}
