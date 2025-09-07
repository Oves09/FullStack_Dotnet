using BankingSystem.Models;
using BankingSystem.Services;
using BankingSystem.Interfaces;

namespace BankingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            BankingService service = new BankingService();
            bool running = true;

            Console.WriteLine("=== Welcome to Advanced Banking System ===\n");

            while (running)
            {
                Console.WriteLine("\n1. Create Account");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Transfer");
                Console.WriteLine("5. Show All Accounts");
                Console.WriteLine("6. Exit");
                Console.Write("Enter choice: ");

                int choice = int.Parse(Console.ReadLine()!);

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter Account Type (Savings/Current): ");
                        string type = Console.ReadLine()!;
                        Console.Write("Enter Holder Name: ");
                        string holder = Console.ReadLine()!;
                        Console.Write("Enter Account Number: ");
                        string accNumber = Console.ReadLine()!;
                        Console.Write("Enter Initial Deposit: ");
                        decimal initDeposit = decimal.Parse(Console.ReadLine()!);
                        service.CreateAccount(type, holder, accNumber, initDeposit);
                        break;

                    case 2:
                        Console.Write("Enter Account Number: ");
                        accNumber = Console.ReadLine()!;
                        var acc = service.FindAccount(accNumber);
                        if (acc != null)
                        {
                            Console.Write("Enter amount: ");
                            decimal amt = decimal.Parse(Console.ReadLine()!);
                            acc.Deposit(amt);
                        }
                        break;

                    case 3:
                        Console.Write("Enter Account Number: ");
                        accNumber = Console.ReadLine()!;
                        acc = service.FindAccount(accNumber);
                        if (acc != null)
                        {
                            Console.Write("Enter amount: ");
                            decimal amt = decimal.Parse(Console.ReadLine()!);
                            acc.Withdraw(amt);
                        }
                        break;

                    case 4:
                        Console.Write("From Account Number: ");
                        string fromAcc = Console.ReadLine()!;
                        Console.Write("To Account Number: ");
                        string toAcc = Console.ReadLine()!;
                        Console.Write("Amount: ");
                        decimal transferAmt = decimal.Parse(Console.ReadLine()!);

                        var fromAccount = service.FindAccount(fromAcc);
                        var toAccount = service.FindAccount(toAcc);

                        if (fromAccount is ITransaction transaction && toAccount != null)
                            transaction.Transfer(toAccount, transferAmt);
                        else
                            Console.WriteLine("Transfer failed! Invalid account or not supported.");
                        break;

                    case 5:
                        service.ShowAllAccounts();
                        break;

                    case 6:
                        running = false;
                        Console.WriteLine("Exiting Banking System. Goodbye!");
                        break;
                }
            }
        }
    }
}
