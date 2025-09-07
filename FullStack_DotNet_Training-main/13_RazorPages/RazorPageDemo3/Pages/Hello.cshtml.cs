using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace RazorPageDemo3.Pages
{
    public class HelloModel : PageModel
    {
        [BindProperty]
        public string? UserName { get; set; }

        public string? Greeting { get; set; }


        // RUns on GET request
        public void OnGet()
        {
            Greeting = "Welcome to Razor Pages Deep Dive!";
        }

        // Runs on Post Request (form submission)

        public void OnPost()
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                Greeting = $"Hello, {UserName}";
            }
            else
            {
                Greeting = "Please enter your name.";
            }
        }
    }
}