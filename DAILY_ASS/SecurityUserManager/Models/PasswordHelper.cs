using System.Security.Cryptography;
using System.Text;

namespace SecureUserManager.Models
{
    public static class PasswordHelper
    {
        public static byte[] GenerateSalt(int size = 16)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[size];
                rng.GetBytes(salt);
                return salt;
            }
        }

        public static byte[] HashPassword(string password, byte[] salt, int iterations = 10000)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                HashAlgorithmName.SHA256
            ))
            {
                return pbkdf2.GetBytes(32); // 256-bit hash
            }
        }

        public static bool VerifyPassword(string password, byte[] salt, byte[] storedHash)
        {
            byte[] newHash = HashPassword(password, salt);
            return FixedTimeEquals(newHash, storedHash);
        }

        // Added missing FixedTimeEquals implementation
        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }
    }
}