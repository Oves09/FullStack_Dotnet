using System;

namespace LibraryManagementSystem.Services
{
    // Delegate for notifications
    public delegate void BookBorrowedEventHandler(string message);

    public class NotificationService
    {
        // Event triggered when a book is borrowed (nullable event fixes warning)
        public event BookBorrowedEventHandler? BookBorrowed;

        public void Notify(string bookTitle, string memberName)
        {
            // Fire event if someone subscribed
            BookBorrowed?.Invoke($"📢 {memberName} borrowed '{bookTitle}' on {DateTime.Now}");
        }
    }
}
