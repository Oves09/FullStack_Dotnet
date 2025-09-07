using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DataBindingDemo.Pages
{
    public class FormDemoModel : PageModel
    {
        [BindProperty]
        public string? Name { get; set; }

        [BindProperty]
        public int Age { get; set; }

        public bool IsSubmitted { get; set; } = false;


        public void OnGet()
        {
            // Nothing here for GET
        }

        public void OnPost()
        {
            IsSubmitted = true;
        }
    }
}