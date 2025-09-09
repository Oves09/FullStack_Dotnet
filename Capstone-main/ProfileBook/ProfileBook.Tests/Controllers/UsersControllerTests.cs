using Microsoft.AspNetCore.Hosting;
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
    public class UsersControllerTests
    {
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<ILogger<UsersController>> _mockLogger;
        private Mock<IWebHostEnvironment> _mockEnvironment;
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            // Setup UserManager mock
            var userStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);

            _mockLogger = new Mock<ILogger<UsersController>>();
            _mockEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new UsersController(_mockUserManager.Object, _mockLogger.Object, _mockEnvironment.Object);

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
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
                {
                    User = principal
                }
            };
        }

        [Test]
        public async Task SearchUsers_WithEmptyQuery_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.SearchUsers("");

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task GetUser_WithValidId_ReturnsUser()
        {
            // Arrange
            var user = new User
            {
                Id = "user1",
                UserName = "testuser",
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _mockUserManager.Setup(x => x.FindByIdAsync("user1"))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            // Act
            var result = await _controller.GetUser("user1");

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            var userDto = okResult.Value as UserDto;
            Assert.That(userDto, Is.Not.Null);
            Assert.That(userDto.UserName, Is.EqualTo("testuser"));
        }

        [Test]
        public async Task UpdateProfile_WithValidData_UpdatesUser()
        {
            // Arrange
            var user = new User
            {
                Id = "test-user-id",
                UserName = "testuser",
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Bio = "Old bio",
                IsActive = true
            };

            _mockUserManager.Setup(x => x.FindByIdAsync("test-user-id"))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            var updateProfileDto = new UpdateProfileDto
            {
                FirstName = "Updated",
                LastName = "Name",
                Bio = "Updated bio"
            };

            // Act
            var result = await _controller.UpdateProfile(updateProfileDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            Assert.That(user.FirstName, Is.EqualTo("Updated"));
            Assert.That(user.LastName, Is.EqualTo("Name"));
            Assert.That(user.Bio, Is.EqualTo("Updated bio"));
        }
    }
}
