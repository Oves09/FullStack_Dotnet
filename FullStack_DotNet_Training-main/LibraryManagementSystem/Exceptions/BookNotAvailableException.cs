using System;

namespace LibraryManagementSystem.Exceptions
{
    public class BookNotAvailableException : Exception
    {
        public BookNotAvailableException(string message) : base(message) { }
    }
}
