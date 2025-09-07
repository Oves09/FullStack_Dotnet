namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public string Title { get; set; } = string.Empty;   // Avoid null warnings
        public string Author { get; set; } = string.Empty;  // Avoid null warnings
        public bool IsAvailable { get; set; } = true;       // Default true when book is created
    }
}
