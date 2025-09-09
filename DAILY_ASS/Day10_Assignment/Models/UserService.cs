using Microsoft.Extensions.Logging;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;


namespace Day10_Assignment.Models
{
    public class UserService
    {
        private static readonly Dictionary<string, string> users = new();

        public static bool Register(string username, string password)
        {
            if (users.ContainsKey(username))
                return false;

            users[username] = HashPassword(password);
            Logger.Log($"User registered: {username}");
            return true;
        }

        public static bool Authenticate(string username, string password)
        {
            return users.TryGetValue(username, out string storedHash) &&
                   storedHash == HashPassword(password);
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return System.Convert.ToBase64String(hash);
        }

    }
}
