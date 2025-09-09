using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileBook.API.Data;
using ProfileBook.API.DTOs;
using ProfileBook.API.Models;
using ProfileBook.API.Services;
using System.Security.Claims;

namespace ProfileBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AdminController> _logger;
        private readonly INotificationService _notificationService;

        public AdminController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<AdminController> logger,
            INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _notificationService = notificationService;
        }

        [HttpGet("pending-posts")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPendingPosts()
        {
            try
            {
                var posts = await _context.Posts
                    .Where(p => p.Status == PostStatus.Pending)
                    .Include(p => p.User)
                    .OrderBy(p => p.CreatedAt)
                    .Select(p => new PostDto
                    {
                        PostId = p.PostId,
                        UserId = p.UserId,
                        UserName = p.User.UserName ?? string.Empty,
                        UserFullName = $"{p.User.FirstName} {p.User.LastName}",
                        UserProfileImage = p.User.ProfileImagePath,
                        Content = p.Content,
                        PostImagePath = p.PostImagePath,
                        Status = p.Status,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching pending posts");
                return StatusCode(500, "An error occurred while fetching pending posts");
            }
        }

        [HttpPost("approve-post")]
        public async Task<IActionResult> ApprovePost(ApprovePostDto approvePostDto)
        {
            try
            {
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized();
                }

                var post = await _context.Posts
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.PostId == approvePostDto.PostId);
                    
                if (post == null)
                {
                    return NotFound();
                }

                post.Status = approvePostDto.IsApproved ? PostStatus.Approved : PostStatus.Rejected;
                post.AdminComments = approvePostDto.AdminComments;
                post.ApprovedByUserId = adminId;
                post.ApprovedAt = DateTime.UtcNow;
                post.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Try to create notification, but don't fail the entire operation if it fails
                try
                {
                    await _notificationService.CreatePostApprovalNotificationAsync(
                        post.UserId, 
                        post.PostId, 
                        approvePostDto.IsApproved
                    );
                }
                catch (Exception notificationEx)
                {
                    _logger.LogWarning(notificationEx, "Failed to create notification for post approval {PostId}, but post was processed successfully", approvePostDto.PostId);
                }

                return Ok(new { message = $"Post {(approvePostDto.IsApproved ? "approved" : "rejected")} successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while approving/rejecting post {PostId}", approvePostDto.PostId);
                return StatusCode(500, "An error occurred while processing the post");
            }
        }


        [HttpPut("users/{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(string id, [FromBody] bool isActive)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.IsActive = isActive;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return Ok(new { message = $"User {(isActive ? "activated" : "deactivated")} successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user status for user {UserId}", id);
                return StatusCode(500, "An error occurred while updating user status");
            }
        }

        [HttpGet("reports")]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetReports(ReportStatus? status = null)
        {
            try
            {
                var query = _context.Reports
                    .Include(r => r.ReportedUser)
                    .Include(r => r.ReportingUser)
                    .Include(r => r.ReviewedByUser)
                    .AsQueryable();

                if (status.HasValue)
                {
                    query = query.Where(r => r.Status == status.Value);
                }

                var reports = await query
                    .OrderByDescending(r => r.TimeStamp)
                    .Select(r => new ReportDto
                    {
                        ReportId = r.ReportId,
                        ReportedUserId = r.ReportedUserId,
                        ReportedUserName = r.ReportedUser.UserName ?? string.Empty,
                        ReportedUserFullName = $"{r.ReportedUser.FirstName} {r.ReportedUser.LastName}",
                        ReportingUserId = r.ReportingUserId,
                        ReportingUserName = r.ReportingUser.UserName ?? string.Empty,
                        ReportingUserFullName = $"{r.ReportingUser.FirstName} {r.ReportingUser.LastName}",
                        Reason = r.Reason,
                        Description = r.Description,
                        TimeStamp = r.TimeStamp,
                        Status = r.Status,
                        AdminNotes = r.AdminNotes,
                        ReviewedByUserName = r.ReviewedByUser != null ? r.ReviewedByUser.UserName : null,
                        ReviewedAt = r.ReviewedAt
                    })
                    .ToListAsync();

                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching reports");
                return StatusCode(500, "An error occurred while fetching reports");
            }
        }

        [HttpPost("review-report")]
        public async Task<IActionResult> ReviewReport(ReviewReportDto reviewReportDto)
        {
            try
            {
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized();
                }

                var report = await _context.Reports.FindAsync(reviewReportDto.ReportId);
                if (report == null)
                {
                    return NotFound();
                }

                report.Status = reviewReportDto.Status;
                report.AdminNotes = reviewReportDto.AdminNotes;
                report.ReviewedByUserId = adminId;
                report.ReviewedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Report reviewed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while reviewing report {ReportId}", reviewReportDto.ReportId);
                return StatusCode(500, "An error occurred while reviewing the report");
            }
        }

        [HttpPost("create-group")]
        public async Task<ActionResult<Group>> CreateGroup([FromBody] CreateGroupDto createGroupDto)
        {
            try
            {
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized();
                }

                var group = new Group
                {
                    GroupName = createGroupDto.GroupName,
                    Description = createGroupDto.Description,
                    CreatedByUserId = adminId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.Groups.Add(group);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetGroup), new { id = group.GroupId }, group);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating group");
                return StatusCode(500, "An error occurred while creating the group");
            }
        }

        [HttpGet("groups/{id}")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
            try
            {
                var group = await _context.Groups
                    .Include(g => g.CreatedByUser)
                    .Include(g => g.UserGroups)
                    .ThenInclude(ug => ug.User)
                    .FirstOrDefaultAsync(g => g.GroupId == id);

                if (group == null)
                {
                    return NotFound();
                }

                return Ok(group);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching group {GroupId}", id);
                return StatusCode(500, "An error occurred while fetching the group");
            }
        }
    }
}
