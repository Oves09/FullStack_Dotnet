namespace ProfileBook.API.DTOs
{
    public class GroupMessageDto
    {
        public int MessageId { get; set; }
        public int GroupId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateGroupMessageDto
    {
        public int GroupId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
