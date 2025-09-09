using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ProfileBook.API.Controllers;
using ProfileBook.API.Data;
using ProfileBook.API.DTOs;
using ProfileBook.API.Models;
using System.Security.Claims;

namespace ProfileBook.Tests.Controllers
{
    [TestFixture]
    public class PostsControllerTests
    {
        private ApplicationDbContext _context;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<ILogger<PostsController>> _mockLogger;
        private Mock<IWebHostEnvironment> _mockEnvironment;
        private PostsController _controller;

        [SetUp]
        public void Setup()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            // Setup UserManager mock
            var userStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);

            _mockLogger = new Mock<ILogger<PostsController>>();
            _mockEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new PostsController(_context, _mockUserManager.Object, _mockLogger.Object, _mockEnvironment.Object);

            // Setup user claims for authorization
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
                new Claim(ClaimTypes.Role, "User")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetPosts_ReturnsApprovedPosts()
        {
            // Arrange
            var user = new User
            {
                Id = "user1",
                UserName = "testuser",
                FirstName = "Test",
                LastName = "User"
            };

            var approvedPost = new Post
            {
                PostId = 1,
                UserId = "user1",
                User = user,
                Content = "Approved post content",
                Status = PostStatus.Approved,
                CreatedAt = DateTime.UtcNow
            };

            var pendingPost = new Post
            {
                PostId = 2,
                UserId = "user1",
                User = user,
                Content = "Pending post content",
                Status = PostStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.Posts.AddRange(approvedPost, pendingPost);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetPosts();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var posts = okResult.Value as IEnumerable<PostDto>;
            Assert.IsNotNull(posts);
            Assert.AreEqual(1, posts.Count()); // Only approved post should be returned
            Assert.AreEqual("Approved post content", posts.First().Content);
        }

        [Test]
        public async Task GetPost_WithValidId_ReturnsPost()
        {
            // Arrange
            var user = new User
            {
                Id = "user1",
                UserName = "testuser",
                FirstName = "Test",
                LastName = "User"
            };

            var post = new Post
            {
                PostId = 1,
                UserId = "user1",
                User = user,
                Content = "Test post content",
                Status = PostStatus.Approved,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetPost(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var postDto = okResult.Value as PostDto;
            Assert.IsNotNull(postDto);
            Assert.AreEqual("Test post content", postDto.Content);
        }

        [Test]
        public async Task CreatePost_WithValidData_CreatesPost()
        {
            // Arrange
            var user = new User
            {
                Id = "test-user-id",
                UserName = "testuser",
                FirstName = "Test",
                LastName = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _mockUserManager.Setup(x => x.FindByIdAsync("test-user-id"))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(false);

            var createPostDto = new CreatePostDto
            {
                Content = "Test post content"
            };

            // Act
            var result = await _controller.CreatePost(createPostDto);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            var postDto = createdResult.Value as PostDto;
            Assert.IsNotNull(postDto);
            Assert.AreEqual("Test post content", postDto.Content);
            Assert.AreEqual(PostStatus.Pending, postDto.Status); // Regular user post should be pending
        }

        [Test]
        public async Task LikePost_WithValidPost_TogglesLike()
        {
            // Arrange
            var user = new User
            {
                Id = "test-user-id",
                UserName = "testuser"
            };

            var post = new Post
            {
                PostId = 1,
                UserId = "other-user",
                Content = "Test post",
                Status = PostStatus.Approved,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Act - First like
            var result1 = await _controller.LikePost(1);

            // Assert - Like added
            var okResult1 = result1 as OkResult;
            Assert.IsNotNull(okResult1);
            Assert.AreEqual(200, okResult1.StatusCode);

            var like = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == 1 && l.UserId == "test-user-id");
            Assert.IsNotNull(like);

            // Act - Second like (unlike)
            var result2 = await _controller.LikePost(1);

            // Assert - Like removed
            var okResult2 = result2 as OkResult;
            Assert.IsNotNull(okResult2);

            var likeAfterUnlike = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == 1 && l.UserId == "test-user-id");
            Assert.IsNull(likeAfterUnlike);
        }

        [Test]
        public async Task GetMyPosts_ReturnsUserPosts()
        {
            // Arrange
            var user = new User
            {
                Id = "test-user-id",
                UserName = "testuser",
                FirstName = "Test",
                LastName = "User"
            };

            var userPost = new Post
            {
                PostId = 1,
                UserId = "test-user-id",
                User = user,
                Content = "My post content",
                Status = PostStatus.Approved,
                CreatedAt = DateTime.UtcNow
            };

            var otherUserPost = new Post
            {
                PostId = 2,
                UserId = "other-user-id",
                Content = "Other user post",
                Status = PostStatus.Approved,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.Posts.AddRange(userPost, otherUserPost);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetMyPosts();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var posts = okResult.Value as IEnumerable<PostDto>;
            Assert.IsNotNull(posts);
            Assert.AreEqual(1, posts.Count()); // Only user's own post
            Assert.AreEqual("My post content", posts.First().Content);
        }
    }
}
