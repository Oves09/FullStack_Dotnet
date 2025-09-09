using System.ComponentModel.DataAnnotations;
using ProfileBook.API.Models;

namespace ProfileBook.API.DTOs
{
    public class CreateReportDto
    {
        [Required]
        public string ReportedUserId { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }
    }

    public class ReportDto
    {
        public int ReportId { get; set; }
        public string ReportedUserId { get; set; } = string.Empty;
        public string ReportedUserName { get; set; } = string.Empty;
        public string ReportedUserFullName { get; set; } = string.Empty;
        public string ReportingUserId { get; set; } = string.Empty;
        public string ReportingUserName { get; set; } = string.Empty;
        public string ReportingUserFullName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime TimeStamp { get; set; }
        public ReportStatus Status { get; set; }
        public string? AdminNotes { get; set; }
        public string? ReviewedByUserName { get; set; }
        public DateTime? ReviewedAt { get; set; }
    }

    public class ReviewReportDto
    {
        [Required]
        public int ReportId { get; set; }

        [Required]
        public ReportStatus Status { get; set; }

        [StringLength(1000)]
        public string? AdminNotes { get; set; }
    }
}
