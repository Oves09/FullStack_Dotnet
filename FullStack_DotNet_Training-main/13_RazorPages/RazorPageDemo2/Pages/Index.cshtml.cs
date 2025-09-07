using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _13._2_RazorPageDemo.Pages;

public class IndexModel : PageModel
{
    public string Message { get; set; }

    public void OnGet()
    {
        Message = "Hello from Razor Page!";
    }
}
