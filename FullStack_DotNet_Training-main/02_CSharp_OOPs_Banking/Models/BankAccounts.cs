namespace BankingSystem.Models
{
    // Abstract class = Abstraction
    public abstract class BankAccount
    {
        // Encapsulation: private balance
        private decimal balance;

        public string AccountHolder { get; private set; }
        public string AccountNumber { get; private set; }

        public BankAccount(string accountHolder, string accountNumber, decimal initialBalance)
        {
            AccountHolder = accountHolder;
            AccountNumber = accountNumber;
            balance = initialBalance;
        }

        // Protected so child classes can access
        protected decimal Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                balance += amount;
                Console.WriteLine($"{amount} deposited. New Balance: {balance}");
            }
        }

        public abstract void Withdraw(decimal amount);

        public virtual void PrintDetails()
        {
            Console.WriteLine($"[Account] Holder: {AccountHolder}, Number: {AccountNumber}, Balance: {balance}");
        }
    }
}
