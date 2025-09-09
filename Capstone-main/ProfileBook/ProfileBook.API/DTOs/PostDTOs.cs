using System.ComponentModel.DataAnnotations;
using ProfileBook.API.Models;

namespace ProfileBook.API.DTOs
{
    public class CreatePostDto
    {
        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        public IFormFile? PostImage { get; set; }
    }

    public class UpdatePostDto
    {
        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        public IFormFile? PostImage { get; set; }
    }

    public class PostDto
    {
        public int PostId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        public string? UserProfileImage { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? PostImagePath { get; set; }
        public PostStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? AdminComments { get; set; }
        public string? ApprovedByUserName { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
    }

    public class ApprovePostDto
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        [StringLength(500)]
        public string? AdminComments { get; set; }
    }
}
