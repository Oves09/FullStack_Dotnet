using ProfileBook.API.Models;

namespace ProfileBook.API.Services
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(string userId, string title, string message, string type = "Info", int? postId = null);
        Task CreatePostApprovalNotificationAsync(string userId, int postId, bool isApproved);
        Task CreateUserCreatedNotificationAsync(string userId);
        Task CreateUserUpdatedNotificationAsync(string userId);
        Task CreateGroupInviteNotificationAsync(string userId, string groupName);
    }
}
