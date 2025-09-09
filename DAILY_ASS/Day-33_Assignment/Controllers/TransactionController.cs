using Microsoft.AspNetCore.Mvc;
using BankingMVC.Models;
using BankingMVC.Filters;

namespace BankingMVC.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    public class TransactionController : Controller
    {
        public IActionResult Dashboard()
        {
            var transactions = new List<Transaction>
            {
                new Transaction{ Id=1, AccountOwner="user", Amount=5000, Date=DateTime.Now, Type="Deposit"},
                new Transaction{ Id=2, AccountOwner="user", Amount=2000, Date=DateTime.Now, Type="Withdraw"}
            };

            return View(transactions);
        }
    }
}
