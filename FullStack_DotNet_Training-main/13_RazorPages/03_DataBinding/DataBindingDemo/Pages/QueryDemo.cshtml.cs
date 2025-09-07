using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DataBindingDemo.Pages
{
    public class QueryDemoModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? Name { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Age { get; set; }

        public void OnGet()
        {
        }
    }
}