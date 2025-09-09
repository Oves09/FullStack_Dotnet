using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileBook.API.DTOs;
using ProfileBook.API.Models;
using System.Security.Claims;

namespace ProfileBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UsersController> _logger;
        private readonly IWebHostEnvironment _environment;

        public UsersController(
            UserManager<User> userManager,
            ILogger<UsersController> logger,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _logger = logger;
            _environment = environment;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserDto>>> SearchUsers(string query, int page = 1, int pageSize = 10)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest("Search query is required");
                }

                var users = await _userManager.Users
                    .Where(u => u.IsActive && 
                               (u.UserName.Contains(query) || 
                                u.FirstName.Contains(query) || 
                                u.LastName.Contains(query) ||
                                u.Email.Contains(query)))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var userDtos = new List<UserDto>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userDtos.Add(new UserDto
                    {
                        Id = user.Id,
                        UserName = user.UserName ?? string.Empty,
                        Email = user.Email ?? string.Empty,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Bio = user.Bio,
                        ProfileImagePath = user.ProfileImagePath,
                        CreatedAt = user.CreatedAt,
                        IsActive = user.IsActive,
                        Roles = roles
                    });
                }

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching users");
                return StatusCode(500, "An error occurred while searching users");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null || !user.IsActive)
                {
                    return NotFound();
                }

                var roles = await _userManager.GetRolesAsync(user);
                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Bio = user.Bio,
                    ProfileImagePath = user.ProfileImagePath,
                    CreatedAt = user.CreatedAt,
                    IsActive = user.IsActive,
                    Roles = roles
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user {UserId}", id);
                return StatusCode(500, "An error occurred while fetching the user");
            }
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                return await GetUser(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user profile");
                return StatusCode(500, "An error occurred while fetching your profile");
            }
        }

        [HttpPut("profile")]
        public async Task<ActionResult<UserDto>> UpdateProfile([FromForm] UpdateProfileDto updateProfileDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                // Update user properties
                user.FirstName = updateProfileDto.FirstName;
                user.LastName = updateProfileDto.LastName;
                user.Bio = updateProfileDto.Bio;
                user.UpdatedAt = DateTime.UtcNow;

                // Handle profile image upload
                if (updateProfileDto.ProfileImage != null)
                {
                    // Delete old profile image if exists
                    if (!string.IsNullOrEmpty(user.ProfileImagePath))
                    {
                        var oldImagePath = Path.Combine(_environment.WebRootPath, user.ProfileImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save new profile image
                    user.ProfileImagePath = await SaveImageAsync(updateProfileDto.ProfileImage, "profiles");
                }

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Bio = user.Bio,
                    ProfileImagePath = user.ProfileImagePath,
                    CreatedAt = user.CreatedAt,
                    IsActive = user.IsActive,
                    Roles = roles
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user profile");
                return StatusCode(500, "An error occurred while updating your profile");
            }
        }

        private async Task<string> SaveImageAsync(IFormFile image, string folder)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{image.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            return $"/uploads/{folder}/{fileName}";
        }
    }

    public class UpdateProfileDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
