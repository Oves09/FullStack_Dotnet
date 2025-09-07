using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DataBindingDemo.Pages
{
    public class CombinedDemoModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? Greeting { get; set; }

        [BindProperty]
        public string? Message { get; set; }

        public bool IsSubmitted { get; set; } = false;

        public void OnGet() { }

        public void OnPost()
        {
            IsSubmitted = true;
        }
    }
}
