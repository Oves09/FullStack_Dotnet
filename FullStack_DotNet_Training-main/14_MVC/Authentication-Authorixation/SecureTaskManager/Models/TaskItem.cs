using System.ComponentModel.DataAnnotations;

namespace SecureTaskManager.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public virtual ApplicationUser User { get; set; } = null!;

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}