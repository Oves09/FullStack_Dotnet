using System.ComponentModel.DataAnnotations;

namespace ProfileBook.API.DTOs
{
    public class CreateMessageDto
    {
        [Required]
        public string ReceiverId { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string MessageContent { get; set; } = string.Empty;
    }

    public class MessageDto
    {
        public int MessageId { get; set; }
        public string SenderId { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;
        public string MessageContent { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public bool IsRead { get; set; }
    }

    public class ConversationDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        public string? UserProfileImage { get; set; }
        public MessageDto? LastMessage { get; set; }
        public int UnreadCount { get; set; }
    }
}
