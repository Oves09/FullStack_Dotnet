using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PageStructureDemo.Pages
{
    public class HelloModel : PageModel
    {
        public string Message { get; private set; } = "Default message";

        public void OnGet()
        {
            Message="page loaded successfully! (OnGet executed)";
        }
    }
}