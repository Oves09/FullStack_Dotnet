using System.ComponentModel.DataAnnotations;

namespace ProfileBook.API.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(500)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string Type { get; set; } = "Info"; // Info, Success, Warning, Error
        
        public bool IsRead { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        
        // Optional reference to related entities
        public int? PostId { get; set; }
        public virtual Post? Post { get; set; }
    }
}
