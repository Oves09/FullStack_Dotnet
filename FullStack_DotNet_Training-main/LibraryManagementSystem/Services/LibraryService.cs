using System;
using System.Collections.Generic;
using LibraryManagementSystem.Exceptions;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class LibraryService
    {
        private List<Book> books = new List<Book>();
        private NotificationService notificationService;

        public LibraryService(NotificationService notification)
        {
            notificationService = notification;
            notificationService.BookBorrowed += OnBookBorrowed;
        }

        private void OnBookBorrowed(string message)
        {
            Console.WriteLine(message);
        }

        public void AddBook(Book book)
        {
            books.Add(book);
            Console.WriteLine($"✅ Book '{book.Title}' added successfully.");
        }

        public void ShowBooks()
        {
            Console.WriteLine("\n📚 Available Books:");
            foreach (var book in books)
            {
                string status = book.IsAvailable ? "Available" : "Borrowed";
                Console.WriteLine($"- {book.Title} by {book.Author} [{status}]");
            }
        }

        public void BorrowBook(string title, string memberName)
        {
            var book = books.Find(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (book == null || !book.IsAvailable)
                throw new BookNotAvailableException("❌ Book not available!");

            book.IsAvailable = false;

            notificationService.Notify(book.Title, memberName);
        }
    }
}
