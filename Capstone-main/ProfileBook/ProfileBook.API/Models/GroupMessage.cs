using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileBook.API.Models
{
    public class GroupMessage
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        [ForeignKey("Group")]
        public int GroupId { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Group Group { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
