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
    public class ReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<ReportsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<ReportDto>> CreateReport(CreateReportDto createReportDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                // Check if user is trying to report themselves
                if (userId == createReportDto.ReportedUserId)
                {
                    return BadRequest("You cannot report yourself");
                }

                // Check if reported user exists
                var reportedUser = await _userManager.FindByIdAsync(createReportDto.ReportedUserId);
                if (reportedUser == null)
                {
                    return BadRequest("Reported user not found");
                }

                // Check if user has already reported this user
                var existingReport = await _context.Reports
                    .FirstOrDefaultAsync(r => r.ReportingUserId == userId && 
                                            r.ReportedUserId == createReportDto.ReportedUserId &&
                                            r.Status == ReportStatus.Pending);

                if (existingReport != null)
                {
                    return BadRequest("You have already reported this user");
                }

                var report = new Report
                {
                    ReportedUserId = createReportDto.ReportedUserId,
                    ReportingUserId = userId,
                    Reason = createReportDto.Reason,
                    Description = createReportDto.Description,
                    TimeStamp = DateTime.UtcNow,
                    Status = ReportStatus.Pending
                };

                _context.Reports.Add(report);
                await _context.SaveChangesAsync();

                var reportingUser = await _userManager.FindByIdAsync(userId);

                var reportDto = new ReportDto
                {
                    ReportId = report.ReportId,
                    ReportedUserId = report.ReportedUserId,
                    ReportedUserName = reportedUser.UserName ?? string.Empty,
                    ReportedUserFullName = $"{reportedUser.FirstName} {reportedUser.LastName}",
                    ReportingUserId = report.ReportingUserId,
                    ReportingUserName = reportingUser?.UserName ?? string.Empty,
                    ReportingUserFullName = $"{reportingUser?.FirstName} {reportingUser?.LastName}",
                    Reason = report.Reason,
                    Description = report.Description,
                    TimeStamp = report.TimeStamp,
                    Status = report.Status
                };

                return CreatedAtAction(nameof(GetReport), new { id = report.ReportId }, reportDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating report");
                return StatusCode(500, "An error occurred while creating the report");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportDto>> GetReport(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var report = await _context.Reports
                    .Include(r => r.ReportedUser)
                    .Include(r => r.ReportingUser)
                    .Include(r => r.ReviewedByUser)
                    .FirstOrDefaultAsync(r => r.ReportId == id);

                if (report == null)
                {
                    return NotFound();
                }

                // Only allow the reporting user or admins to view the report
                var userRoles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(userId));
                if (report.ReportingUserId != userId && !userRoles.Contains("Admin"))
                {
                    return Forbid();
                }

                var reportDto = new ReportDto
                {
                    ReportId = report.ReportId,
                    ReportedUserId = report.ReportedUserId,
                    ReportedUserName = report.ReportedUser.UserName ?? string.Empty,
                    ReportedUserFullName = $"{report.ReportedUser.FirstName} {report.ReportedUser.LastName}",
                    ReportingUserId = report.ReportingUserId,
                    ReportingUserName = report.ReportingUser.UserName ?? string.Empty,
                    ReportingUserFullName = $"{report.ReportingUser.FirstName} {report.ReportingUser.LastName}",
                    Reason = report.Reason,
                    Description = report.Description,
                    TimeStamp = report.TimeStamp,
                    Status = report.Status,
                    AdminNotes = report.AdminNotes,
                    ReviewedByUserName = report.ReviewedByUser?.UserName,
                    ReviewedAt = report.ReviewedAt
                };

                return Ok(reportDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching report {ReportId}", id);
                return StatusCode(500, "An error occurred while fetching the report");
            }
        }

        [HttpGet("my-reports")]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetMyReports()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var reports = await _context.Reports
                    .Where(r => r.ReportingUserId == userId)
                    .Include(r => r.ReportedUser)
                    .Include(r => r.ReportingUser)
                    .Include(r => r.ReviewedByUser)
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
                _logger.LogError(ex, "Error occurred while fetching user reports");
                return StatusCode(500, "An error occurred while fetching your reports");
            }
        }
    }
}
