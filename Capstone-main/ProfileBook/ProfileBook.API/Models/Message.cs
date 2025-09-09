using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileBook.API.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        [ForeignKey("Sender")]
        public string SenderId { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string MessageContent { get; set; } = string.Empty;

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        public virtual User Sender { get; set; } = null!;
        public virtual User Receiver { get; set; } = null!;
    }
}
