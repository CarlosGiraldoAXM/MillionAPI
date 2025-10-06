using NUnit.Framework;
using FluentAssertions;
using MillionAPI.Application.DTOs;
using MillionAPI.Application.Services;
using Moq;
using Microsoft.Extensions.Options;

namespace MillionAPI.Tests;

[TestFixture]
public class AuthServiceTests
{
    private Mock<IOptions<JwtSettings>> _mockJwtSettings;
    private Mock<IOptions<TestUser>> _mockTestUser;
    private AuthService _authService;

    [SetUp]
    public void Setup()
    {
        _mockJwtSettings = new Mock<IOptions<JwtSettings>>();
        _mockTestUser = new Mock<IOptions<TestUser>>();
        
        // Configurar JWT settings
        var jwtSettings = new JwtSettings
        {
            SecretKey = "MillionAPI_SecretKey_2024_SuperSecretKey_ForJWT_Token_Generation_And_Validation",
            Issuer = "MillionAPI",
            Audience = "MillionAPI_Users",
            ExpirationInMinutes = 60
        };
        _mockJwtSettings.Setup(x => x.Value).Returns(jwtSettings);
        
        // Configurar TestUser
        var testUser = new TestUser
        {
            Username = "admin",
            Password = "admin123",
            Email = "admin@millionapi.com"
        };
        _mockTestUser.Setup(x => x.Value).Returns(testUser);

        _authService = new AuthService(_mockJwtSettings.Object, _mockTestUser.Object);
    }

    [Test]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var loginRequest = new LoginRequestDto { Username = "admin", Password = "admin123" };

        // Act
        var result = await _authService.LoginAsync(loginRequest);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
        result.Username.Should().Be("admin");
        result.Email.Should().Be("admin@millionapi.com");
        result.Expiration.Should().BeAfter(DateTime.UtcNow);
    }

    [Test]
    public async Task LoginAsync_WithInvalidCredentials_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var loginRequest = new LoginRequestDto { Username = "invalid", Password = "wrong" };

        // Act & Assert
        await _authService.Invoking(x => x.LoginAsync(loginRequest))
                         .Should().ThrowAsync<UnauthorizedAccessException>()
                         .WithMessage("Credenciales inválidas");
    }
}