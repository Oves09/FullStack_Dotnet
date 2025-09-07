using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ValidationDemo.Models;

namespace ValidationDemo.Pages
{
    public class UserFormModel : PageModel
    {
        [BindProperty]
        public User User { get; set; } = new User();

        public string SuccessMessage { get; set; } = "";

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // reload form with validation message
            }

            SuccessMessage = $"User {User.Name} registered successfully!";
            return RedirectToPage("Success", new { name = User.Name });
        }
    }
}