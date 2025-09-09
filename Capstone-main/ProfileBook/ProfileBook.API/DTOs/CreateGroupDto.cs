using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProfileBook.API.DTOs
{
    public class CreateGroupDto
    {
        [Required]
        [StringLength(100)]
        public string GroupName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [JsonPropertyName("memberIds")]
        public List<string> MemberIds { get; set; } = new List<string>();
    }
}
