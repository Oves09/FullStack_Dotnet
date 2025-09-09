using System.ComponentModel.DataAnnotations;

namespace ProfileBook.API.DTOs
{
    public class ResetPasswordDto
    {
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
