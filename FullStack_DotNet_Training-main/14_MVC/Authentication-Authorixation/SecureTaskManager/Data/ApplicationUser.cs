using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SecureTaskManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}