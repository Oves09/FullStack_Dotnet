using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace TempDataViewDataDemo.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            // Passing data to page using ViewData
            ViewData["TitleMessage"] = "Welcome to TempData & ViewData Demo!";
        }

        public IActionResult OnPostRedirect()
        {
            // Passing data using TempData to another page
            TempData["SuccessMessage"] = "This message comes from Index page using TempData!";
            return RedirectToPage("About");
        }
    }
}
