namespace BankingMVC.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public decimal Balance { get; set; }
        public string Role { get; set; } // "User" or "Admin"
    }
}
