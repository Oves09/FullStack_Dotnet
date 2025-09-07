using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LayoutDemo.Pages
{
    public class ContactModel : PageModel
    {
        public string Message { get; set; } = "Your contact page.";

        public void OnGet()
        {
            // You can set properties for the Razor page here
        }
    }
}
