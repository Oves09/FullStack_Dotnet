namespace BankingMVC.Services
{
    public class AuthService : IAuthService
    {
        // Dummy users (replace with DB in real app)
        private readonly Dictionary<string, (string Password, string Role)> _users =
            new Dictionary<string, (string, string)>
            {
                { "admin", ("admin123", "Admin") },
                { "user", ("user123", "User") }
            };

        public bool Login(string username, string password)
        {
            return _users.ContainsKey(username) && _users[username].Password == password;
        }

        public string GetRole(string username)
        {
            return _users.ContainsKey(username) ? _users[username].Role : "User";
        }
    }
}
