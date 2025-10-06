using NUnit.Framework;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MillionAPI.Presentation.Controllers;
using MillionAPI.Application.Interfaces;
using MillionAPI.Application.DTOs;

namespace MillionAPI.Tests;

[TestFixture]
public class AuthControllerTests
{
    private Mock<IAuthService> _mockAuthService;
    private AuthController _controller;

    [SetUp]
    public void Setup()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthController(_mockAuthService.Object);
    }

    [Test]
    public async Task Login_WithValidCredentials_ShouldReturnOk()
    {
        // Arrange
        var loginRequest = new LoginRequestDto { Username = "admin", Password = "admin123" };
        var loginResponse = new LoginResponseDto { Token = "token123", Username = "admin", Email = "admin@test.com", Expiration = DateTime.UtcNow.AddMinutes(60) };
        _mockAuthService.Setup(x => x.LoginAsync(loginRequest)).ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequestDto { Username = "invalid", Password = "wrong" };
        _mockAuthService.Setup(x => x.LoginAsync(loginRequest)).ThrowsAsync(new UnauthorizedAccessException("Credenciales inválidas"));

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Test]
    public async Task Login_WithException_ShouldReturnBadRequest()
    {
        // Arrange
        var loginRequest = new LoginRequestDto { Username = "admin", Password = "admin123" };
        _mockAuthService.Setup(x => x.LoginAsync(loginRequest)).ThrowsAsync(new Exception("Error interno"));

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}