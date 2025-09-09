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
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PostsController> _logger;
        private readonly IWebHostEnvironment _environment;

        public PostsController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<PostsController> logger,
            IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _environment = environment;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts(int page = 1, int pageSize = 10)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                var posts = await _context.Posts
                    .Where(p => p.Status == PostStatus.Approved)
                    .Include(p => p.User)
                    .Include(p => p.Likes)
                    .Include(p => p.Comments)
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
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
                        UpdatedAt = p.UpdatedAt,
                        LikesCount = p.Likes.Count,
                        CommentsCount = p.Comments.Count,
                        IsLikedByCurrentUser = currentUserId != null && p.Likes.Any(l => l.UserId == currentUserId)
                    })
                    .ToListAsync();

                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching posts");
                return StatusCode(500, "An error occurred while fetching posts");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(int id)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                var post = await _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Likes)
                    .Include(p => p.Comments)
                    .Where(p => p.PostId == id && p.Status == PostStatus.Approved)
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
                        UpdatedAt = p.UpdatedAt,
                        LikesCount = p.Likes.Count,
                        CommentsCount = p.Comments.Count,
                        IsLikedByCurrentUser = currentUserId != null && p.Likes.Any(l => l.UserId == currentUserId)
                    })
                    .FirstOrDefaultAsync();

                if (post == null)
                {
                    return NotFound();
                }

                return Ok(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching post {PostId}", id);
                return StatusCode(500, "An error occurred while fetching the post");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PostDto>> CreatePost([FromForm] CreatePostDto createPostDto)
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
                    return Unauthorized();
                }

                string? imagePath = null;
                if (createPostDto.PostImage != null)
                {
                    imagePath = await SaveImageAsync(createPostDto.PostImage, "posts");
                }

                // Check if user is admin - auto-approve admin posts
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                
                var post = new Post
                {
                    UserId = userId,
                    Content = createPostDto.Content,
                    PostImagePath = imagePath,
                    Status = isAdmin ? PostStatus.Approved : PostStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                var postDto = new PostDto
                {
                    PostId = post.PostId,
                    UserId = post.UserId,
                    UserName = user.UserName ?? string.Empty,
                    UserFullName = $"{user.FirstName} {user.LastName}",
                    UserProfileImage = user.ProfileImagePath,
                    Content = post.Content,
                    PostImagePath = post.PostImagePath,
                    Status = post.Status,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt,
                    LikesCount = 0,
                    CommentsCount = 0,
                    IsLikedByCurrentUser = false
                };

                return CreatedAtAction(nameof(GetPost), new { id = post.PostId }, postDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating post");
                return StatusCode(500, "An error occurred while creating the post");
            }
        }

        [HttpPost("{id}/like")]
        public async Task<IActionResult> LikePost(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var post = await _context.Posts.FindAsync(id);
                if (post == null || post.Status != PostStatus.Approved)
                {
                    return NotFound();
                }

                var existingLike = await _context.Likes
                    .FirstOrDefaultAsync(l => l.PostId == id && l.UserId == userId);

                if (existingLike != null)
                {
                    _context.Likes.Remove(existingLike);
                }
                else
                {
                    var like = new Like
                    {
                        PostId = id,
                        UserId = userId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Likes.Add(like);
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while liking/unliking post {PostId}", id);
                return StatusCode(500, "An error occurred while processing the like");
            }
        }

        [HttpGet("my-posts")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetMyPosts()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var posts = await _context.Posts
                    .Where(p => p.UserId == userId)
                    .Include(p => p.User)
                    .Include(p => p.Likes)
                    .Include(p => p.Comments)
                    .OrderByDescending(p => p.CreatedAt)
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
                        UpdatedAt = p.UpdatedAt,
                        AdminComments = p.AdminComments,
                        LikesCount = p.Likes.Count,
                        CommentsCount = p.Comments.Count,
                        IsLikedByCurrentUser = p.Likes.Any(l => l.UserId == userId)
                    })
                    .ToListAsync();

                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user posts");
                return StatusCode(500, "An error occurred while fetching your posts");
            }
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(int id)
        {
            try
            {
                var post = await _context.Posts.FindAsync(id);
                if (post == null || post.Status != PostStatus.Approved)
                {
                    return NotFound();
                }

                var comments = await _context.Comments
                    .Where(c => c.PostId == id && !c.IsDeleted)
                    .Include(c => c.User)
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => new CommentDto
                    {
                        CommentId = c.CommentId,
                        PostId = c.PostId,
                        UserId = c.UserId,
                        UserName = c.User.UserName ?? string.Empty,
                        UserFullName = $"{c.User.FirstName} {c.User.LastName}",
                        UserProfileImage = c.User.ProfileImagePath,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching comments for post {PostId}", id);
                return StatusCode(500, "An error occurred while fetching comments");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var post = await _context.Posts.FindAsync(id);
                if (post == null)
                {
                    return NotFound();
                }

                // Delete associated image file if exists
                if (!string.IsNullOrEmpty(post.PostImagePath))
                {
                    var imagePath = Path.Combine(_environment.WebRootPath, post.PostImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Admin deleted post {PostId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting post {PostId}", id);
                return StatusCode(500, "An error occurred while deleting the post");
            }
        }

        [HttpPost("{id}/comments")]
        public async Task<ActionResult<CommentDto>> AddComment(int id, [FromBody] CreateCommentDto createCommentDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var post = await _context.Posts.FindAsync(id);
                if (post == null || post.Status != PostStatus.Approved)
                {
                    return NotFound();
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Unauthorized();
                }

                var comment = new Comment
                {
                    PostId = id,
                    UserId = userId,
                    Content = createCommentDto.Content,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                var commentDto = new CommentDto
                {
                    CommentId = comment.CommentId,
                    PostId = comment.PostId,
                    UserId = comment.UserId,
                    UserName = user.UserName ?? string.Empty,
                    UserFullName = $"{user.FirstName} {user.LastName}",
                    UserProfileImage = user.ProfileImagePath,
                    Content = comment.Content,
                    CreatedAt = comment.CreatedAt,
                    UpdatedAt = comment.UpdatedAt
                };

                return CreatedAtAction(nameof(GetComments), new { id = comment.PostId }, commentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding comment to post {PostId}", id);
                return StatusCode(500, "An error occurred while adding the comment");
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
}
