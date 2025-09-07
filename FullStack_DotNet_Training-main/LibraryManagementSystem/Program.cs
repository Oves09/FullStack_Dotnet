using System;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

class Program
{
    static void Main(string[] args)
    {
        NotificationService notificationService = new NotificationService();
        LibraryService library = new LibraryService(notificationService);

        while (true)
        {
            Console.WriteLine("\n===== Library Menu =====");
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. Show Books");
            Console.WriteLine("3. Borrow Book");
            Console.WriteLine("0. Exit");
            Console.Write("Enter choice: ");

            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter Title: ");
                        string title = Console.ReadLine();
                        Console.Write("Enter Author: ");
                        string author = Console.ReadLine();
                        library.AddBook(new Book { Title = title, Author = author, IsAvailable = true });
                        break;

                    case "2":
                        library.ShowBooks();
                        break;

                    case "3":
                        Console.Write("Enter Book Title: ");
                        string borrowTitle = Console.ReadLine();
                        Console.Write("Enter Your Name: ");
                        string member = Console.ReadLine();
                        library.BorrowBook(borrowTitle, member);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("⚠️ Invalid choice! Try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
