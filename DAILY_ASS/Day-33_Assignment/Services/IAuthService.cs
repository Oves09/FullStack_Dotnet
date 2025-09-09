namespace BankingMVC.Services
{
    public interface IAuthService
    {
        bool Login(string username, string password);
        string GetRole(string username);
    }
}
