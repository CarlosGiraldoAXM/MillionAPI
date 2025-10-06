using NUnit.Framework;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MillionAPI.Application.DTOs;
using MillionAPI.Application.Interfaces;
using MillionAPI.Presentation.Controllers;
using Moq;

namespace MillionAPI.Tests;

[TestFixture]
public class PropertiesControllerTests
{
    private Mock<IPropertyUseCases> _mockPropertyUseCases;
    private PropertiesController _controller;

    [SetUp]
    public void Setup()
    {
        _mockPropertyUseCases = new Mock<IPropertyUseCases>();
        _controller = new PropertiesController(_mockPropertyUseCases.Object);
    }

    [Test]
    public async Task GetProperties_WithFilters_ShouldReturnOk()
    {
        // Arrange
        var filters = new PropertyFilterDto { Name = "Casa" };
        var properties = new List<PropertyDto> { new PropertyDto { Id = 1, Name = "Casa en Madrid", Price = 250000 } };
        _mockPropertyUseCases.Setup(x => x.GetPropertiesWithFiltersAsync(filters)).ReturnsAsync(properties);

        // Act
        var result = await _controller.GetProperties(filters);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task CreateProperty_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var createDto = new CreatePropertyDto { Name = "Casa en Madrid", Address = "Calle Mayor 123", Price = 250000, CodeInternal = "PROP001", OwnerId = 1, Year = 2020 };
        var propertyDto = new PropertyDto { Id = 1, Name = "Casa en Madrid", Address = "Calle Mayor 123", Price = 250000, CodeInternal = "PROP001", OwnerId = 1, Year = 2020 };
        _mockPropertyUseCases.Setup(x => x.CreatePropertyAsync(createDto)).ReturnsAsync(propertyDto);

        // Act
        var result = await _controller.CreateProperty(createDto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Test]
    public async Task UpdatePropertyPrice_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var propertyId = 1;
        var updatePriceDto = new UpdatePropertyPriceDto { Price = 300000 };
        _mockPropertyUseCases.Setup(x => x.UpdatePropertyPriceAsync(propertyId, updatePriceDto)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdatePropertyPrice(propertyId, updatePriceDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
}