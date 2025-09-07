using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorSyntaxApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public int Age { get; set; }

        public void OnGet()
        {
            // Page load logic
        }

        public void OnPost()
        {
            // Razor automatically binds Age from the form input
        }
    }
}
