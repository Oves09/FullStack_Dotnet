using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileBook.API.Data;
using ProfileBook.API.DTOs;
using ProfileBook.API.Models;
using System.Security.Claims;

namespace ProfileBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<MessagesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("conversations")]
        public async Task<ActionResult<IEnumerable<ConversationDto>>> GetConversations()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var conversations = await _context.Messages
                    .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                    .Where(m => !m.IsDeleted)
                    .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        LastMessage = g.OrderByDescending(m => m.TimeStamp).First(),
                        UnreadCount = g.Count(m => m.ReceiverId == userId && !m.IsRead)
                    })
                    .ToListAsync();

                var conversationDtos = new List<ConversationDto>();
                foreach (var conv in conversations)
                {
                    var user = await _userManager.FindByIdAsync(conv.UserId);
                    if (user != null)
                    {
                        conversationDtos.Add(new ConversationDto
                        {
                            UserId = conv.UserId,
                            UserName = user.UserName ?? string.Empty,
                            UserFullName = $"{user.FirstName} {user.LastName}",
                            UserProfileImage = user.ProfileImagePath,
                            LastMessage = new MessageDto
                            {
                                MessageId = conv.LastMessage.MessageId,
                                SenderId = conv.LastMessage.SenderId,
                                SenderName = conv.LastMessage.SenderId == userId ? "You" : user.UserName ?? string.Empty,
                                ReceiverId = conv.LastMessage.ReceiverId,
                                ReceiverName = conv.LastMessage.ReceiverId == userId ? "You" : user.UserName ?? string.Empty,
                                MessageContent = conv.LastMessage.MessageContent,
                                TimeStamp = conv.LastMessage.TimeStamp,
                                IsRead = conv.LastMessage.IsRead
                            },
                            UnreadCount = conv.UnreadCount
                        });
                    }
                }

                return Ok(conversationDtos.OrderByDescending(c => c.LastMessage?.TimeStamp));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching conversations");
                return StatusCode(500, "An error occurred while fetching conversations");
            }
        }

        [HttpGet("conversation/{otherUserId}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetConversation(string otherUserId, int page = 1, int pageSize = 50)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var messages = await _context.Messages
                    .Where(m => (m.SenderId == userId && m.ReceiverId == otherUserId) ||
                               (m.SenderId == otherUserId && m.ReceiverId == userId))
                    .Where(m => !m.IsDeleted)
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .OrderByDescending(m => m.TimeStamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(m => new MessageDto
                    {
                        MessageId = m.MessageId,
                        SenderId = m.SenderId,
                        SenderName = m.Sender.UserName ?? string.Empty,
                        ReceiverId = m.ReceiverId,
                        ReceiverName = m.Receiver.UserName ?? string.Empty,
                        MessageContent = m.MessageContent,
                        TimeStamp = m.TimeStamp,
                        IsRead = m.IsRead
                    })
                    .ToListAsync();

                // Mark messages as read
                var unreadMessages = await _context.Messages
                    .Where(m => m.SenderId == otherUserId && m.ReceiverId == userId && !m.IsRead)
                    .ToListAsync();

                foreach (var message in unreadMessages)
                {
                    message.IsRead = true;
                }

                if (unreadMessages.Any())
                {
                    await _context.SaveChangesAsync();
                }

                return Ok(messages.OrderBy(m => m.TimeStamp));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching conversation with user {OtherUserId}", otherUserId);
                return StatusCode(500, "An error occurred while fetching the conversation");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> SendMessage(CreateMessageDto createMessageDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var receiver = await _userManager.FindByIdAsync(createMessageDto.ReceiverId);
                if (receiver == null || !receiver.IsActive)
                {
                    return BadRequest("Receiver not found or inactive");
                }

                var sender = await _userManager.FindByIdAsync(userId);
                if (sender == null)
                {
                    return Unauthorized();
                }

                var message = new Message
                {
                    SenderId = userId,
                    ReceiverId = createMessageDto.ReceiverId,
                    MessageContent = createMessageDto.MessageContent,
                    TimeStamp = DateTime.UtcNow,
                    IsRead = false,
                    IsDeleted = false
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                var messageDto = new MessageDto
                {
                    MessageId = message.MessageId,
                    SenderId = message.SenderId,
                    SenderName = sender.UserName ?? string.Empty,
                    ReceiverId = message.ReceiverId,
                    ReceiverName = receiver.UserName ?? string.Empty,
                    MessageContent = message.MessageContent,
                    TimeStamp = message.TimeStamp,
                    IsRead = message.IsRead
                };

                return CreatedAtAction(nameof(GetMessage), new { id = message.MessageId }, messageDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending message");
                return StatusCode(500, "An error occurred while sending the message");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MessageDto>> GetMessage(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var message = await _context.Messages
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .Where(m => m.MessageId == id && 
                               (m.SenderId == userId || m.ReceiverId == userId) && 
                               !m.IsDeleted)
                    .Select(m => new MessageDto
                    {
                        MessageId = m.MessageId,
                        SenderId = m.SenderId,
                        SenderName = m.Sender.UserName ?? string.Empty,
                        ReceiverId = m.ReceiverId,
                        ReceiverName = m.Receiver.UserName ?? string.Empty,
                        MessageContent = m.MessageContent,
                        TimeStamp = m.TimeStamp,
                        IsRead = m.IsRead
                    })
                    .FirstOrDefaultAsync();

                if (message == null)
                {
                    return NotFound();
                }

                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching message {MessageId}", id);
                return StatusCode(500, "An error occurred while fetching the message");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var message = await _context.Messages
                    .FirstOrDefaultAsync(m => m.MessageId == id && m.SenderId == userId);

                if (message == null)
                {
                    return NotFound();
                }

                message.IsDeleted = true;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Message deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting message {MessageId}", id);
                return StatusCode(500, "An error occurred while deleting the message");
            }
        }
    }
}
