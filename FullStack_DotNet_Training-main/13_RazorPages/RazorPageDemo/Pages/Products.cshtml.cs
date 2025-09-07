using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace RazorPageDemo.Pages
{
    public class ProductsModel : PageModel
    {
        public List<string> ProductList { get; set; } = new List<string>();

        public void OnGet()
        {
            ProductList = new List<string> { "Laptop", "Phone", "Tablet", "Mouse", "Keyboard", "Pendrive" };
        }
    }
}