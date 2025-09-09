using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using ProfileBook.API.Models;
using ProfileBook.API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProfileBook.Tests.Services
{
    [TestFixture]
    public class TokenServiceTests
    {
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<UserManager<User>> _mockUserManager;
        private TokenService _tokenService;

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            
            // Setup UserManager mock
            var userStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);

            // Setup JWT configuration
            _mockConfiguration.Setup(x => x["Jwt:Key"]).Returns("ThisIsASecretKeyForJWTTokenGenerationThatIsLongEnough");
            _mockConfiguration.Setup(x => x["Jwt:Issuer"]).Returns("ProfileBookAPI");
            _mockConfiguration.Setup(x => x["Jwt:Audience"]).Returns("ProfileBookClient");
            _mockConfiguration.Setup(x => x["Jwt:ExpireHours"]).Returns("24");

            _tokenService = new TokenService(_mockConfiguration.Object, _mockUserManager.Object);
        }

        [Test]
        public async Task GenerateTokenAsync_WithValidUser_ReturnsValidToken()
        {
            // Arrange
            var user = new User
            {
                Id = "user-id-123",
                UserName = "testuser",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User"
            };

            var roles = new List<string> { "User" };
            _mockUserManager.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(roles);

            // Act
            var token = await _tokenService.GenerateTokenAsync(user);

            // Assert
            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.Not.Empty);

            // Verify token can be read
            var tokenHandler = new JwtSecurityTokenHandler();
            Assert.That(tokenHandler.CanReadToken(token), Is.True);

            var jwtToken = tokenHandler.ReadJwtToken(token);
            
            // Verify basic token structure
            Assert.That(jwtToken.Claims.Count(), Is.GreaterThan(0));
            
            // Verify at least one claim exists (the token is valid)
            var hasClaims = jwtToken.Claims.Any();
            Assert.That(hasClaims, Is.True);
        }

        [Test]
        public async Task GenerateTokenAsync_WithAdminUser_ReturnsValidToken()
        {
            // Arrange
            var adminUser = new User
            {
                Id = "admin-id-123",
                UserName = "admin",
                Email = "admin@example.com",
                FirstName = "Admin",
                LastName = "User"
            };

            var roles = new List<string> { "Admin", "User" };
            _mockUserManager.Setup(x => x.GetRolesAsync(adminUser))
                .ReturnsAsync(roles);

            // Act
            var token = await _tokenService.GenerateTokenAsync(adminUser);

            // Assert
            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.Not.Empty);

            var tokenHandler = new JwtSecurityTokenHandler();
            Assert.That(tokenHandler.CanReadToken(token), Is.True);
            
            var jwtToken = tokenHandler.ReadJwtToken(token);
            Assert.That(jwtToken.Claims.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void GenerateTokenAsync_WithMissingJwtKey_ThrowsException()
        {
            // Arrange
            _mockConfiguration.Setup(x => x["Jwt:Key"]).Returns((string?)null);
            var tokenServiceWithoutKey = new TokenService(_mockConfiguration.Object, _mockUserManager.Object);

            var user = new User
            {
                Id = "user-id-123",
                UserName = "testuser",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User"
            };

            _mockUserManager.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => 
                await tokenServiceWithoutKey.GenerateTokenAsync(user));
        }

        [Test]
        public async Task GenerateTokenAsync_TokenExpirationTime_IsCorrect()
        {
            // Arrange
            var user = new User
            {
                Id = "user-id-123",
                UserName = "testuser",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User"
            };

            _mockUserManager.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            var beforeTokenGeneration = DateTime.UtcNow;

            // Act
            var token = await _tokenService.GenerateTokenAsync(user);

            // Assert
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            
            var expectedExpiration = beforeTokenGeneration.AddHours(24);
            var actualExpiration = jwtToken.ValidTo;
            
            // Allow for a small time difference (1 minute) due to processing time
            var timeDifference = Math.Abs((expectedExpiration - actualExpiration).TotalMinutes);
            Assert.IsTrue(timeDifference < 1, $"Token expiration time difference is too large: {timeDifference} minutes");
        }
    }
}
