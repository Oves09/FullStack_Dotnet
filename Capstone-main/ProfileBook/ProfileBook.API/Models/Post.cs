using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileBook.API.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        public string? PostImagePath { get; set; }

        [Required]
        public PostStatus Status { get; set; } = PostStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public string? AdminComments { get; set; }

        public string? ApprovedByUserId { get; set; }

        public DateTime? ApprovedAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual User? ApprovedByUser { get; set; }
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }

    public enum PostStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}
