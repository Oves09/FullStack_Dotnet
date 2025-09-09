using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesProductsApp.Models;
using System.Collections.Generic;

namespace RazorPagesProductsApp.Pages.Products
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Product NewProduct { get; set; } = new Product();

        public static List<Product> Products { get; set; } = new List<Product>();

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            Products.Add(NewProduct);
            return RedirectToPage();
        }
    }
}
