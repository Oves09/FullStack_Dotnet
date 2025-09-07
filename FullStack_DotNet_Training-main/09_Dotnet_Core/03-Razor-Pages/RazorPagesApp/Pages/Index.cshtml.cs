using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string? UserName { get; set; }

        public string? Message { get; set; }

        public void OnGet()
        {
            Message = string.Empty;
        }

        public void OnPost()
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                Message = $"Hello, {UserName}!";
            }
            else
            {
                Message = "Please enter your name.";
            }
        }
    }
}
