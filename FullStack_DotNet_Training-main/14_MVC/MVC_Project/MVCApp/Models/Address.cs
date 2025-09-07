using System.ComponentModel.DataAnnotations;

namespace MVCApp.Models
{
    public class Address
    {
        [Required]
        [StringLength(200)]
        public string Street { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid ZIP code format")]
        public string ZipCode { get; set; } = string.Empty;
    }
}