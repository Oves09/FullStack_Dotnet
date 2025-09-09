using System.Collections.Generic;

namespace RazorPagesProductsApp.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public List<Category> Categories { get; set; } = new List<Category>();
    }

    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = "";
    }
}
