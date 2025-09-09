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
    public class GroupsControllerTests
    {
        private ApplicationDbContext _context;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<ILogger<GroupsController>> _mockLogger;
        private GroupsController _controller;

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

            _mockLogger = new Mock<ILogger<GroupsController>>();

            _controller = new GroupsController(_context, _mockUserManager.Object, _mockLogger.Object);

            // Setup user claims for authorization
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
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
        public async Task GetGroups_ReturnsActiveGroups()
        {
            // Arrange
            var user = new User
            {
                Id = "user1",
                UserName = "testuser"
            };

            var activeGroup = new Group
            {
                GroupId = 1,
                GroupName = "Active Group",
                Description = "Active group description",
                CreatedByUserId = "user1",
                CreatedByUser = user,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var inactiveGroup = new Group
            {
                GroupId = 2,
                GroupName = "Inactive Group",
                Description = "Inactive group description",
                CreatedByUserId = "user1",
                CreatedByUser = user,
                IsActive = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.Groups.AddRange(activeGroup, inactiveGroup);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetGroups();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            var groups = okResult.Value as IEnumerable<GroupDto>;
            Assert.That(groups, Is.Not.Null);
            Assert.That(groups.Count(), Is.EqualTo(1)); // Only active group should be returned
            Assert.That(groups.First().GroupName, Is.EqualTo("Active Group"));
        }

        [Test]
        public async Task GetGroup_WithValidId_ReturnsGroup()
        {
            // Arrange
            var user = new User
            {
                Id = "user1",
                UserName = "testuser"
            };

            var group = new Group
            {
                GroupId = 1,
                GroupName = "Test Group",
                Description = "Test description",
                CreatedByUserId = "user1",
                CreatedByUser = user,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetGroup(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            var groupDto = okResult.Value as GroupDto;
            Assert.That(groupDto, Is.Not.Null);
            Assert.That(groupDto.GroupName, Is.EqualTo("Test Group"));
        }

        [Test]
        public async Task DeleteGroup_WithValidId_SetsInactive()
        {
            // Arrange
            var group = new Group
            {
                GroupId = 1,
                GroupName = "Test Group",
                Description = "Test description",
                CreatedByUserId = "user1",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteGroup(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            // Verify group is marked as inactive
            var updatedGroup = await _context.Groups.FindAsync(1);
            Assert.That(updatedGroup, Is.Not.Null);
            Assert.That(updatedGroup.IsActive, Is.False);
        }
    }
}
