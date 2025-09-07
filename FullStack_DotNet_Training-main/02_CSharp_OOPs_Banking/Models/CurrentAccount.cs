using BankingSystem.Interfaces;

namespace BankingSystem.Models
{
    public class CurrentAccount : BankAccount, ITransaction
    {
        private const decimal OverdraftLimit = 5000;

        public CurrentAccount(string accountHolder, string accountNumber, decimal initialBalance)
            : base(accountHolder, accountNumber, initialBalance) { }

        // Polymorphism: Current accounts allow overdraft
        public override void Withdraw(decimal amount)
        {
            if (Balance - amount >= -OverdraftLimit)
            {
                Balance -= amount;
                Console.WriteLine($"{amount} withdrawn from Current. Remaining: {Balance}");
            }
            else
            {
                Console.WriteLine("Withdrawal failed! Overdraft limit exceeded.");
            }
        }

        // Interface Implementation
        public void Transfer(BankAccount toAccount, decimal amount)
        {
            if (Balance - amount >= -OverdraftLimit)
            {
                Balance -= amount;
                toAccount.Deposit(amount);
                Console.WriteLine($"Transferred {amount} to {toAccount.AccountHolder}");
            }
            else
            {
                Console.WriteLine("Transfer failed! Overdraft limit exceeded.");
            }
        }
    }
}
