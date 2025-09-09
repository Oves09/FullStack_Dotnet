using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileBook.API.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        [ForeignKey("ReportedUser")]
        public string ReportedUserId { get; set; } = string.Empty;

        [Required]
        [ForeignKey("ReportingUser")]
        public string ReportingUserId { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        public ReportStatus Status { get; set; } = ReportStatus.Pending;

        public string? AdminNotes { get; set; }

        public string? ReviewedByUserId { get; set; }

        public DateTime? ReviewedAt { get; set; }

        // Navigation properties
        public virtual User ReportedUser { get; set; } = null!;
        public virtual User ReportingUser { get; set; } = null!;
        public virtual User? ReviewedByUser { get; set; }
    }

    public enum ReportStatus
    {
        Pending = 0,
        UnderReview = 1,
        Resolved = 2,
        Dismissed = 3
    }
}
