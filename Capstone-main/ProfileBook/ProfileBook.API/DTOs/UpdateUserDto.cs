using System.ComponentModel.DataAnnotations;

namespace ProfileBook.API.DTOs
{
    public class UpdateUserDto
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        public bool IsActive { get; set; } = true;

        public List<string>? Roles { get; set; }
    }
}
