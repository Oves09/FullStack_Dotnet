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
using ProfileBook.API.Services;
using System.Security.Claims;

namespace ProfileBook.Tests.Controllers
{
    [TestFixture]
    public class AdminControllerTests
    {
        private ApplicationDbContext _context;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<ILogger<AdminController>> _mockLogger;
        private Mock<INotificationService> _mockNotificationService;
        private AdminController _controller;

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

            _mockLogger = new Mock<ILogger<AdminController>>();
            _mockNotificationService = new Mock<INotificationService>();

            _controller = new AdminController(_context, _mockUserManager.Object, _mockLogger.Object, _mockNotificationService.Object);

            // Setup user claims for authorization
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "admin-user-id"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
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
        public async Task GetPendingPosts_ReturnsPendingPosts()
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
                Status = PostStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetPendingPosts();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var posts = okResult.Value as IEnumerable<PostDto>;
            Assert.IsNotNull(posts);
            Assert.AreEqual(1, posts.Count());
            Assert.AreEqual("Test post content", posts.First().Content);
        }

        [Test]
        public async Task ApprovePost_WithValidData_ApprovesPost()
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
                Status = PostStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            var approvePostDto = new ApprovePostDto
            {
                PostId = 1,
                IsApproved = true,
                AdminComments = "Post approved"
            };

            _mockNotificationService.Setup(x => x.CreatePostApprovalNotificationAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ApprovePost(approvePostDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var updatedPost = await _context.Posts.FindAsync(1);
            Assert.AreEqual(PostStatus.Approved, updatedPost.Status);
            Assert.AreEqual("Post approved", updatedPost.AdminComments);
        }

        [Test]
        public async Task UpdateUserStatus_WithValidUser_UpdatesStatus()
        {
            // Arrange
            var user = new User
            {
                Id = "user1",
                UserName = "testuser",
                IsActive = true
            };

            _mockUserManager.Setup(x => x.FindByIdAsync("user1"))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.UpdateUserStatus("user1", false);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsFalse(user.IsActive);
        }

        [Test]
        public async Task CreateGroup_WithValidData_CreatesGroup()
        {
            // Arrange
            var createGroupDto = new CreateGroupDto
            {
                GroupName = "Test Group",
                Description = "Test group description"
            };

            // Act
            var result = await _controller.CreateGroup(createGroupDto);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            var group = createdResult.Value as Group;
            Assert.IsNotNull(group);
            Assert.AreEqual("Test Group", group.GroupName);
            Assert.AreEqual("Test group description", group.Description);
        }
    }
}
