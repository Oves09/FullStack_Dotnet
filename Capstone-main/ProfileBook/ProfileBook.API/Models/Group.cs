using System.ComponentModel.DataAnnotations;

namespace ProfileBook.API.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        [StringLength(100)]
        public string GroupName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public string CreatedByUserId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual User CreatedByUser { get; set; } = null!;
        public virtual ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
    }
}
