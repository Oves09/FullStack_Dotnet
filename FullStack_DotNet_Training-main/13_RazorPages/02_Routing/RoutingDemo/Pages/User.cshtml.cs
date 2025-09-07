using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RoutingDemo.Pages
{
    public class UserModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int? UserId { get; set; }  //Bound from Router Parameter

        public void OnGet(){}
    }
}