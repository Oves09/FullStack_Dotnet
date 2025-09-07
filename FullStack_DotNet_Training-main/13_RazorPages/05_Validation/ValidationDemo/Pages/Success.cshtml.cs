using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ValidationDemo.Pages
{
    public class SuccessModel : PageModel
    {
        public string Name { get; set; } = "";

        public void OnGet(string name)
        {
            Name = name;
        }
    }
}
