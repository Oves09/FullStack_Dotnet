namespace BankingSystem.Models
{
    public class SavingsAccount : BankAccount
    {
        private const decimal MinimumBalance = 2000;

        public SavingsAccount(string accountHolder, string accountNumber, decimal initialBalance)
            : base(accountHolder, accountNumber, initialBalance) { }

        // Polymorphism: Custom withdraw rule
        public override void Withdraw(decimal amount)
        {
            if (Balance - amount >= MinimumBalance)
            {
                Balance -= amount;
                Console.WriteLine($"{amount} withdrawn from Savings. Remaining: {Balance}");
            }
            else
            {
                Console.WriteLine("Withdrawal failed! Minimum balance requirement not met.");
            }
        }
    }
}
