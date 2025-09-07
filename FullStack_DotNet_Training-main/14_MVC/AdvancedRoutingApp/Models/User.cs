namespace AdvancedRoutingApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public List<Order> Orders { get; set; } = new List<Order>();
    }

    public class Order
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}