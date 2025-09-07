using BankingSystem.Models;


namespace BankingSystem.Interfaces
{
    // Interface defines contract for transactions
    public interface ITransaction
    {
        void Transfer(BankAccount toAccount, decimal amount);
    }
}
