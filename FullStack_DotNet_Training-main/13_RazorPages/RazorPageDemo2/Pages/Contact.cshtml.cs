using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ContactModel : PageModel
{
    [BindProperty]
    public string Name { get; set; }

    public void OnGet() { }

    public void OnPost()
    {
        // 'Name' property automatically bound from form input
    }
}
