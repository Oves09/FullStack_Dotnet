using SecureUserManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SecureUserManager
{
    class Program
    {
        static void Main(string[] args)
        {
            // Generate global encryption key (demo only - use secure storage in production)
            byte[] encryptionKey = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(encryptionKey);
            }

            List<User> users = new List<User>();

            while (true)
            {
                try
                {
                    Console.WriteLine("\n1. Register\n2. Login\n3. Exit");
                    Console.Write("Choose an option: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1": // Register
                            Console.Write("Username: ");
                            string regUser = Console.ReadLine() ?? "";
                            Console.Write("Password: ");
                            string regPass = Console.ReadLine() ?? "";

                            if (string.IsNullOrWhiteSpace(regUser) || string.IsNullOrWhiteSpace(regPass))
                            {
                                Logger.LogError("Registration failed: Empty input");
                                Console.WriteLine("Username/password cannot be empty.");
                                break;
                            }

                            byte[] salt = PasswordHelper.GenerateSalt();
                            byte[] hashedPassword = PasswordHelper.HashPassword(regPass, salt);

                            users.Add(new User
                            {
                                Username = regUser,
                                Salt = salt,
                                HashedPassword = hashedPassword
                            });

                            Logger.Log($"User registered: {regUser}");
                            Console.WriteLine("Registration successful!");
                            break;

                        case "2": // Login
                            Console.Write("Username: ");
                            string loginUser = Console.ReadLine() ?? "";
                            Console.Write("Password: ");
                            string loginPass = Console.ReadLine() ?? "";

                            User user = users.FirstOrDefault(u => u.Username == loginUser);
                            if (user == null)
                            {
                                Logger.LogError($"Login failed: User '{loginUser}' not found");
                                Console.WriteLine("Invalid credentials.");
                                break;
                            }

                            bool isValid = PasswordHelper.VerifyPassword(loginPass, user.Salt, user.HashedPassword);
                            if (isValid)
                            {
                                Logger.Log($"User logged in: {loginUser}");
                                Console.WriteLine("Login successful!");
                            }
                            else
                            {
                                Logger.LogError($"Login failed: Invalid password for '{loginUser}'");
                                Console.WriteLine("Invalid credentials.");
                            }
                            break;

                        case "3": // Exit
                            return;

                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Exception: {ex.Message}");
                    Console.WriteLine("An error occurred. Please try again.");
                }
            }
        }
    }
}