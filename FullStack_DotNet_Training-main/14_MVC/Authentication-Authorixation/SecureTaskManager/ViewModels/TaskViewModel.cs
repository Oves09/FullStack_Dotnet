using SecureTaskManager.Models;
using System.ComponentModel.DataAnnotations;

namespace SecureTaskManager.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-._()]+$", ErrorMessage = "Title contains invalid characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public TaskPriority Priority { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}