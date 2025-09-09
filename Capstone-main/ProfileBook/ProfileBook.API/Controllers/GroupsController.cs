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
    [Authorize(Roles = "Admin")]
    public class GroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<GroupsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetGroups(int page = 1, int pageSize = 10)
        {
            try
            {
                var groups = await _context.Groups
                    .Include(g => g.CreatedByUser)
                    .Include(g => g.UserGroups)
                        .ThenInclude(ug => ug.User)
                    .Where(g => g.IsActive)
                    .OrderByDescending(g => g.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(g => new GroupDto
                    {
                        GroupId = g.GroupId,
                        GroupName = g.GroupName,
                        Description = g.Description,
                        CreatedByUserId = g.CreatedByUserId,
                        CreatedByUserName = g.CreatedByUser.UserName ?? string.Empty,
                        CreatedAt = g.CreatedAt,
                        UpdatedAt = g.UpdatedAt,
                        IsActive = g.IsActive,
                        MemberCount = g.UserGroups.Count,
                        Members = g.UserGroups.Select(ug => new UserDto
                        {
                            Id = ug.User.Id,
                            UserName = ug.User.UserName ?? string.Empty,
                            Email = ug.User.Email ?? string.Empty,
                            FirstName = ug.User.FirstName,
                            LastName = ug.User.LastName,
                            ProfileImagePath = ug.User.ProfileImagePath,
                            IsActive = ug.User.IsActive
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(groups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching groups");
                return StatusCode(500, new { message = "An error occurred while fetching groups" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDto>> GetGroup(int id)
        {
            try
            {
                var group = await _context.Groups
                    .Include(g => g.CreatedByUser)
                    .Include(g => g.UserGroups)
                        .ThenInclude(ug => ug.User)
                    .Where(g => g.GroupId == id && g.IsActive)
                    .Select(g => new GroupDto
                    {
                        GroupId = g.GroupId,
                        GroupName = g.GroupName,
                        Description = g.Description,
                        CreatedByUserId = g.CreatedByUserId,
                        CreatedByUserName = g.CreatedByUser.UserName ?? string.Empty,
                        CreatedAt = g.CreatedAt,
                        UpdatedAt = g.UpdatedAt,
                        IsActive = g.IsActive,
                        MemberCount = g.UserGroups.Count,
                        Members = g.UserGroups.Select(ug => new UserDto
                        {
                            Id = ug.User.Id,
                            UserName = ug.User.UserName ?? string.Empty,
                            Email = ug.User.Email ?? string.Empty,
                            FirstName = ug.User.FirstName,
                            LastName = ug.User.LastName,
                            ProfileImagePath = ug.User.ProfileImagePath,
                            IsActive = ug.User.IsActive
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (group == null)
                {
                    return NotFound();
                }

                return Ok(group);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching group {GroupId}", id);
                return StatusCode(500, new { message = "An error occurred while fetching the group" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<GroupDto>> CreateGroup(CreateGroupDto createGroupDto)
        {
            _logger.LogInformation("CreateGroup called with data: {@CreateGroupDto}", createGroupDto);
            _logger.LogInformation("MemberIds count: {Count}", createGroupDto.MemberIds?.Count ?? 0);
            
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Try multiple claim types for user ID
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst("sub")?.Value 
                           ?? User.FindFirst("nameid")?.Value;
                           
                _logger.LogInformation("User ID from claims: {UserId}", userId);
                _logger.LogInformation("All claims: {@Claims}", User.Claims.Select(c => new { c.Type, c.Value }));
                
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Unauthorized access attempt - no user ID in claims");
                    _logger.LogWarning("Available claims: {@Claims}", User.Claims.Select(c => new { c.Type, c.Value }));
                    return Unauthorized(new { message = "User ID not found in token claims" });
                }

                // Validate member IDs first, before creating the group
                if (createGroupDto.MemberIds != null && createGroupDto.MemberIds.Any())
                {
                    var validUserIds = await _userManager.Users
                        .Where(u => createGroupDto.MemberIds.Contains(u.Id) && u.IsActive)
                        .Select(u => u.Id)
                        .ToListAsync();

                    if (validUserIds.Count != createGroupDto.MemberIds.Count)
                    {
                        var invalidIds = createGroupDto.MemberIds.Except(validUserIds).ToList();
                        _logger.LogWarning("Invalid or inactive user IDs found during group creation: {InvalidIds}", string.Join(", ", invalidIds));
                        return BadRequest(new { message = $"Some users are invalid or inactive: {string.Join(", ", invalidIds)}" });
                    }
                }

                var group = new Group
                {
                    GroupName = createGroupDto.GroupName,
                    Description = createGroupDto.Description,
                    CreatedByUserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.Groups.Add(group);
                await _context.SaveChangesAsync(); // Save group first to get GroupId
                
                _logger.LogInformation("Group created with ID: {GroupId}", group.GroupId);

                // Add members to the group
                if (createGroupDto.MemberIds != null && createGroupDto.MemberIds.Any())
                {
                    _logger.LogInformation("Adding {Count} members to group {GroupId}", createGroupDto.MemberIds.Count, group.GroupId);
                    
                    // Use the already validated member IDs from earlier
                    var validUserIds = await _userManager.Users
                        .Where(u => createGroupDto.MemberIds.Contains(u.Id) && u.IsActive)
                        .Select(u => u.Id)
                        .ToListAsync();
                    
                    _logger.LogInformation("Valid user IDs for group membership: {@ValidUserIds}", validUserIds);

                    if (validUserIds.Any())
                    {
                        var userGroups = validUserIds.Select(memberId => new UserGroup
                        {
                            UserId = memberId,
                            GroupId = group.GroupId,
                            JoinedAt = DateTime.UtcNow,
                            IsActive = true
                        }).ToList();

                        _logger.LogInformation("Adding {Count} UserGroup entities", userGroups.Count);
                        _context.UserGroups.AddRange(userGroups);
                        
                        try
                        {
                            await _context.SaveChangesAsync();
                            _logger.LogInformation("Successfully added members to group {GroupId}", group.GroupId);
                        }
                        catch (Exception memberEx)
                        {
                            _logger.LogError(memberEx, "Error adding members to group {GroupId}: {Error}", group.GroupId, memberEx.Message);
                            throw; // Re-throw to trigger transaction rollback
                        }
                    }
                    else
                    {
                        _logger.LogWarning("No valid users found to add to group {GroupId}", group.GroupId);
                    }
                }

                await transaction.CommitAsync();
                return await GetGroup(group.GroupId);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating group: {Message}. Full exception: {Exception}", ex.Message, ex.ToString());
                _logger.LogError("Stack trace: {StackTrace}", ex.StackTrace);
                return StatusCode(500, new { message = "An error occurred while creating the group", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GroupDto>> UpdateGroup(int id, UpdateGroupDto updateGroupDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var group = await _context.Groups
                    .Include(g => g.UserGroups)
                    .FirstOrDefaultAsync(g => g.GroupId == id && g.IsActive);

                if (group == null)
                {
                    return NotFound();
                }

                // Validate member IDs first, before making any changes
                if (updateGroupDto.MemberIds != null && updateGroupDto.MemberIds.Any())
                {
                    var validUserIds = await _userManager.Users
                        .Where(u => updateGroupDto.MemberIds.Contains(u.Id) && u.IsActive)
                        .Select(u => u.Id)
                        .ToListAsync();

                    if (validUserIds.Count != updateGroupDto.MemberIds.Count)
                    {
                        var invalidIds = updateGroupDto.MemberIds.Except(validUserIds).ToList();
                        _logger.LogWarning("Invalid or inactive user IDs found: {InvalidIds}", string.Join(", ", invalidIds));
                        return BadRequest(new { message = $"Some users are invalid or inactive: {string.Join(", ", invalidIds)}" });
                    }
                }

                group.GroupName = updateGroupDto.GroupName;
                group.Description = updateGroupDto.Description;
                group.UpdatedAt = DateTime.UtcNow;

                // Update group members
                var existingMembers = group.UserGroups.ToList();
                _context.UserGroups.RemoveRange(existingMembers);

                if (updateGroupDto.MemberIds != null && updateGroupDto.MemberIds.Any())
                {
                    var validUserIds = await _userManager.Users
                        .Where(u => updateGroupDto.MemberIds.Contains(u.Id) && u.IsActive)
                        .Select(u => u.Id)
                        .ToListAsync();

                    var newUserGroups = validUserIds.Select(memberId => new UserGroup
                    {
                        UserId = memberId,
                        GroupId = group.GroupId,
                        JoinedAt = DateTime.UtcNow,
                        IsActive = true
                    }).ToList();

                    _context.UserGroups.AddRange(newUserGroups);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetGroup(id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while updating group {GroupId}", id);
                return StatusCode(500, new { message = "An error occurred while updating the group" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            try
            {
                var group = await _context.Groups.FindAsync(id);
                if (group == null || !group.IsActive)
                {
                    return NotFound();
                }

                group.IsActive = false;
                group.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Group deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting group {GroupId}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the group" });
            }
        }
    }
}
