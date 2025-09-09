namespace BankingMVC.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string AccountOwner { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } // Deposit, Withdraw, Transfer
    }
}
