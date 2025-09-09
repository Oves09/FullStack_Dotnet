using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ProfileBook.API.Controllers;
using ProfileBook.API.DTOs;
using ProfileBook.API.Models;
using ProfileBook.API.Services;

namespace ProfileBook.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<SignInManager<User>> _mockSignInManager;
        private Mock<ITokenService> _mockTokenService;
        private Mock<ILogger<AuthController>> _mockLogger;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            // Setup UserManager mock
            var userStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);

            // Setup SignInManager mock
            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            _mockSignInManager = new Mock<SignInManager<User>>(_mockUserManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);

            _mockTokenService = new Mock<ITokenService>();
            _mockLogger = new Mock<ILogger<AuthController>>();

            _controller = new AuthController(_mockUserManager.Object, _mockSignInManager.Object, _mockTokenService.Object, _mockLogger.Object);
        }

        [Test]
        public async Task Register_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "Test123!",
                FirstName = "Test",
                LastName = "User",
                Bio = "Test bio"
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email))
                .ReturnsAsync((User)null);
            _mockUserManager.Setup(x => x.FindByNameAsync(registerDto.UserName))
                .ReturnsAsync((User)null);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), "User"))
                .ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { "User" });
            _mockTokenService.Setup(x => x.GenerateTokenAsync(It.IsAny<User>()))
                .ReturnsAsync("test-token");

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            Assert.IsInstanceOf<ActionResult<AuthResponseDto>>(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as AuthResponseDto;
            Assert.IsNotNull(response);
            Assert.AreEqual("test-token", response.Token);
            Assert.AreEqual(registerDto.UserName, response.User.UserName);
        }

        [Test]
        public async Task Register_WithExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                UserName = "testuser",
                Email = "existing@example.com",
                Password = "Test123!",
                FirstName = "Test",
                LastName = "User"
            };

            var existingUser = new User { Email = registerDto.Email };
            _mockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task Login_WithValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                UserName = "testuser",
                Password = "Test123!"
            };

            var user = new User
            {
                Id = "1",
                UserName = loginDto.UserName,
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.UserName))
                .ReturnsAsync(user);
            _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(user, loginDto.Password, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _mockUserManager.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });
            _mockTokenService.Setup(x => x.GenerateTokenAsync(user))
                .ReturnsAsync("test-token");

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as AuthResponseDto;
            Assert.IsNotNull(response);
            Assert.AreEqual("test-token", response.Token);
            Assert.AreEqual(loginDto.UserName, response.User.UserName);
        }

        [Test]
        public async Task Login_WithInvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                UserName = "nonexistentuser",
                Password = "Test123!"
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.UserName))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var unauthorizedResult = result.Result as UnauthorizedObjectResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }

        [Test]
        public async Task Logout_ReturnsOkResult()
        {
            // Arrange
            _mockSignInManager.Setup(x => x.SignOutAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Logout();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }
    }
}
