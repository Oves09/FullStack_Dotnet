namespace ECommerceApp.Models.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalAmount => Items.Sum(i => (i.Product?.Price ?? 0) * i.Quantity);
        public int TotalItems => Items.Sum(i => i.Quantity);
    }
}