using Microsoft.EntityFrameworkCore;
using ProfileBook.API.Data;
using ProfileBook.API.Models;

namespace ProfileBook.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ApplicationDbContext context, ILogger<NotificationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateNotificationAsync(string userId, string title, string message, string type = "Info", int? postId = null)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = userId,
                    Title = title,
                    Message = message,
                    Type = type,
                    PostId = postId,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Notification created for user {UserId}: {Title}", userId, title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating notification for user {UserId}", userId);
                throw;
            }
        }

        public async Task CreatePostApprovalNotificationAsync(string userId, int postId, bool isApproved)
        {
            var title = isApproved ? "Post Approved!" : "Post Rejected";
            var message = isApproved 
                ? "Your post has been approved by an administrator and is now visible to other users."
                : "Your post has been rejected by an administrator. Please review our community guidelines.";
            var type = isApproved ? "Success" : "Warning";

            await CreateNotificationAsync(userId, title, message, type, postId);
        }

        public async Task CreateUserCreatedNotificationAsync(string userId)
        {
            const string title = "Welcome to ProfileBook!";
            const string message = "Your account has been successfully created. Welcome to our community!";
            const string type = "Success";

            await CreateNotificationAsync(userId, title, message, type);
        }

        public async Task CreateUserUpdatedNotificationAsync(string userId)
        {
            const string title = "Profile Updated";
            const string message = "Your profile information has been successfully updated.";
            const string type = "Info";

            await CreateNotificationAsync(userId, title, message, type);
        }

        public async Task CreateGroupInviteNotificationAsync(string userId, string groupName)
        {
            var title = "Group Invitation";
            var message = $"You have been added to the group '{groupName}'.";
            const string type = "Info";

            await CreateNotificationAsync(userId, title, message, type);
        }
    }
}
