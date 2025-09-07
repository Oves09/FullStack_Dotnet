using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ModelBindingDemo.Pages
{
    public class UserModel : PageModel
    {
        //Query string Binding
        [BindProperty(SupportsGet = true)]
        public string? QueryName { get; set; }

        //Route data binding
        [BindProperty(SupportsGet = true)]
        public int? UserId { get; set; }

        // FOrm binding
        public string? Email { get; set; }

        public void OnGet()
        {
            //Values are automatically bound
        }

        public void OnPost()
        {
            //Email will be populated from here
        }
    }
}