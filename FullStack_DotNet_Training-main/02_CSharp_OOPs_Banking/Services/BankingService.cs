using BankingSystem.Interfaces;
using BankingSystem.Models;

namespace BankingSystem.Services
{
    public class BankingService
    {
        private readonly List<BankAccount> accounts = new();

        public void CreateAccount(string type, string holder, string accNumber, decimal initialDeposit)
        {
            BankAccount account;
            if (type == "Savings")
                account = new SavingsAccount(holder, accNumber, initialDeposit);
            else
                account = new CurrentAccount(holder, accNumber, initialDeposit);

            accounts.Add(account);
            Console.WriteLine($"{type} account created for {holder}");
        }

        public BankAccount? FindAccount(string accNumber)
        {
            return accounts.FirstOrDefault(a => a.AccountNumber == accNumber);
        }

        public void ShowAllAccounts()
        {
            foreach (var acc in accounts)
                acc.PrintDetails();
        }
    }
}
