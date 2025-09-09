using Day26_assignment2_razorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Day26_assignment2_razorPages.Pages.Items
{
    public class ListModel : PageModel
    {
        private readonly ItemService _service;
        public List<string> Items { get; set; }

        public ListModel(ItemService service)
        {
            _service = service;
        }

        public void OnGet()
        {
            Items = _service.GetItems();
        }
    }
}
