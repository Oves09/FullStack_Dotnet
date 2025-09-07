namespace CSharpOops.Topics
{
    public class Encapsulation
    {
        public static void Run()
        {
            Console.WriteLine("--- Encapsulation Example ---");

            BankAccount account = new BankAccount();
            account.Deposit(500);
            account.Withdraw(200);
            Console.WriteLine("Balance: " + account.GetBalance());
        }
    }

    // Class encapsulates data (balance) and restricts direct access
    class BankAccount
    {
        private decimal balance; // private field

        // Public method to deposit money
        public void Deposit(decimal amount)
        {
            if (amount > 0)
                balance += amount;
        }

        // Public method to withdraw money safely
        public void Withdraw(decimal amount)
        {
            if (amount <= balance)
                balance -= amount;
        }

        // Public method to read balance
        public decimal GetBalance()
        {
            return balance;
        }
    }
}
