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
    public class UserGroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserGroupsController> _logger;

        public UserGroupsController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<UserGroupsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // Get groups for the current user
        [HttpGet("my-groups")]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetMyGroups()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst("sub")?.Value 
                           ?? User.FindFirst("nameid")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token claims" });
                }

                var groups = await _context.UserGroups
                    .Include(ug => ug.Group)
                        .ThenInclude(g => g.CreatedByUser)
                    .Include(ug => ug.Group)
                        .ThenInclude(g => g.UserGroups)
                            .ThenInclude(ug => ug.User)
                    .Where(ug => ug.UserId == userId && ug.IsActive && ug.Group.IsActive)
                    .Select(ug => new GroupDto
                    {
                        GroupId = ug.Group.GroupId,
                        GroupName = ug.Group.GroupName,
                        Description = ug.Group.Description,
                        CreatedByUserId = ug.Group.CreatedByUserId,
                        CreatedByUserName = ug.Group.CreatedByUser.UserName ?? string.Empty,
                        CreatedAt = ug.Group.CreatedAt,
                        UpdatedAt = ug.Group.UpdatedAt,
                        IsActive = ug.Group.IsActive,
                        MemberCount = ug.Group.UserGroups.Count(x => x.IsActive),
                        Members = ug.Group.UserGroups
                            .Where(x => x.IsActive)
                            .Select(x => new UserDto
                            {
                                Id = x.User.Id,
                                UserName = x.User.UserName ?? string.Empty,
                                Email = x.User.Email ?? string.Empty,
                                FirstName = x.User.FirstName,
                                LastName = x.User.LastName,
                                ProfileImagePath = x.User.ProfileImagePath,
                                IsActive = x.User.IsActive
                            }).ToList()
                    })
                    .ToListAsync();

                return Ok(groups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user groups");
                return StatusCode(500, new { message = "An error occurred while fetching your groups" });
            }
        }

        // Get specific group details (only if user is a member)
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDto>> GetGroup(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst("sub")?.Value 
                           ?? User.FindFirst("nameid")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token claims" });
                }

                // Check if user is a member of this group
                var userGroup = await _context.UserGroups
                    .Include(ug => ug.Group)
                        .ThenInclude(g => g.CreatedByUser)
                    .Include(ug => ug.Group)
                        .ThenInclude(g => g.UserGroups)
                            .ThenInclude(ug => ug.User)
                    .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GroupId == id && ug.IsActive && ug.Group.IsActive);

                if (userGroup == null)
                {
                    return NotFound(new { message = "Group not found or you are not a member of this group" });
                }

                var groupDto = new GroupDto
                {
                    GroupId = userGroup.Group.GroupId,
                    GroupName = userGroup.Group.GroupName,
                    Description = userGroup.Group.Description,
                    CreatedByUserId = userGroup.Group.CreatedByUserId,
                    CreatedByUserName = userGroup.Group.CreatedByUser.UserName ?? string.Empty,
                    CreatedAt = userGroup.Group.CreatedAt,
                    UpdatedAt = userGroup.Group.UpdatedAt,
                    IsActive = userGroup.Group.IsActive,
                    MemberCount = userGroup.Group.UserGroups.Count(x => x.IsActive),
                    Members = userGroup.Group.UserGroups
                        .Where(x => x.IsActive)
                        .Select(x => new UserDto
                        {
                            Id = x.User.Id,
                            UserName = x.User.UserName ?? string.Empty,
                            Email = x.User.Email ?? string.Empty,
                            FirstName = x.User.FirstName,
                            LastName = x.User.LastName,
                            ProfileImagePath = x.User.ProfileImagePath,
                            IsActive = x.User.IsActive
                        }).ToList()
                };

                return Ok(groupDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching group {GroupId}", id);
                return StatusCode(500, new { message = "An error occurred while fetching the group" });
            }
        }

        // Get messages for a specific group (only if user is a member)
        [HttpGet("{id}/messages")]
        public async Task<ActionResult<IEnumerable<GroupMessageDto>>> GetGroupMessages(int id, int page = 1, int pageSize = 50)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst("sub")?.Value 
                           ?? User.FindFirst("nameid")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token claims" });
                }

                // Check if user is a member of this group
                var isMember = await _context.UserGroups
                    .AnyAsync(ug => ug.UserId == userId && ug.GroupId == id && ug.IsActive);

                if (!isMember)
                {
                    return Forbid("You are not a member of this group");
                }

                var messages = await _context.GroupMessages
                    .Include(gm => gm.User)
                    .Where(gm => gm.GroupId == id && gm.IsActive)
                    .OrderByDescending(gm => gm.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(gm => new GroupMessageDto
                    {
                        MessageId = gm.MessageId,
                        GroupId = gm.GroupId,
                        UserId = gm.UserId,
                        UserName = gm.User.UserName ?? string.Empty,
                        UserFullName = $"{gm.User.FirstName} {gm.User.LastName}".Trim(),
                        Content = gm.Content,
                        CreatedAt = gm.CreatedAt
                    })
                    .ToListAsync();

                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching messages for group {GroupId}", id);
                return StatusCode(500, new { message = "An error occurred while fetching group messages" });
            }
        }

        // Send a message to a group (only if user is a member)
        [HttpPost("{id}/messages")]
        public async Task<ActionResult<GroupMessageDto>> SendGroupMessage(int id, CreateGroupMessageDto createMessageDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst("sub")?.Value 
                           ?? User.FindFirst("nameid")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token claims" });
                }

                // Check if user is a member of this group
                var userGroup = await _context.UserGroups
                    .Include(ug => ug.User)
                    .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GroupId == id && ug.IsActive);

                if (userGroup == null)
                {
                    return Forbid("You are not a member of this group");
                }

                // Validate message content
                if (string.IsNullOrWhiteSpace(createMessageDto.Content))
                {
                    return BadRequest(new { message = "Message content cannot be empty" });
                }

                var groupMessage = new GroupMessage
                {
                    GroupId = id,
                    UserId = userId,
                    Content = createMessageDto.Content.Trim(),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.GroupMessages.Add(groupMessage);
                await _context.SaveChangesAsync();

                var messageDto = new GroupMessageDto
                {
                    MessageId = groupMessage.MessageId,
                    GroupId = groupMessage.GroupId,
                    UserId = groupMessage.UserId,
                    UserName = userGroup.User.UserName ?? string.Empty,
                    UserFullName = $"{userGroup.User.FirstName} {userGroup.User.LastName}".Trim(),
                    Content = groupMessage.Content,
                    CreatedAt = groupMessage.CreatedAt
                };

                return CreatedAtAction(nameof(GetGroupMessages), new { id = id }, messageDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending message to group {GroupId}", id);
                return StatusCode(500, new { message = "An error occurred while sending the message" });
            }
        }
    }
}
