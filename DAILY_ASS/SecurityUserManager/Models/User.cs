using System;
using System.Security.Cryptography;

namespace SecureUserManager.Models
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public byte[] Salt { get; set; } = Array.Empty<byte>();
        public byte[] HashedPassword { get; set; } = Array.Empty<byte>();
    }
}