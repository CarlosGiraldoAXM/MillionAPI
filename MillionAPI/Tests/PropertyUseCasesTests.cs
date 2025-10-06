using NUnit.Framework;
using FluentAssertions;
using MillionAPI.Application.DTOs;
using MillionAPI.Application.Interfaces;
using MillionAPI.Application.UseCases;
using MillionAPI.Domain.Entities;
using MillionAPI.Domain.Interfaces;
using MillionAPI.Application.Services;
using Moq;
using Microsoft.AspNetCore.Http;
using AutoMapper;

namespace MillionAPI.Tests;

[TestFixture]
public class PropertyUseCasesTests
{
    private Mock<IPropertyRepository> _mockPropertyRepository;
    private Mock<IOwnerRepository> _mockOwnerRepository;
    private Mock<IPropertyImageRepository> _mockPropertyImageRepository;
    private Mock<IMapper> _mockMapper;
    private Mock<IFileService> _mockFileService;
    private PropertyUseCases _propertyUseCases;

    [SetUp]
    public void Setup()
    {
        _mockPropertyRepository = new Mock<IPropertyRepository>();
        _mockOwnerRepository = new Mock<IOwnerRepository>();
        _mockPropertyImageRepository = new Mock<IPropertyImageRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockFileService = new Mock<IFileService>();

        _propertyUseCases = new PropertyUseCases(
            _mockPropertyRepository.Object,
            _mockOwnerRepository.Object,
            _mockPropertyImageRepository.Object,
            _mockMapper.Object,
            _mockFileService.Object);
    }

    [Test]
    public async Task CreatePropertyAsync_WithValidData_ShouldReturnPropertyDto()
    {
        // Arrange
        var createDto = new CreatePropertyDto { Name = "Casa en Madrid", Address = "Calle Mayor 123", Price = 250000, CodeInternal = "PROP001", OwnerId = 1, Year = 2020 };
        var property = new Property("Casa en Madrid", "Calle Mayor 123", 250000, "PROP001", 1, 2020);
        var propertyDto = new PropertyDto { Id = 1, Name = "Casa en Madrid", Address = "Calle Mayor 123", Price = 250000, CodeInternal = "PROP001", OwnerId = 1, Year = 2020, OwnerName = "Juan Pérez", ImagesCount = 0 };

        _mockOwnerRepository.Setup(x => x.ExistsAsync(1)).ReturnsAsync(true);
        _mockPropertyRepository.Setup(x => x.ExistsByCodeInternal("PROP001")).Returns(false);
        _mockPropertyRepository.Setup(x => x.AddAsync(It.IsAny<Property>())).ReturnsAsync(property);
        _mockPropertyRepository.Setup(x => x.BeginTransactionAsync()).ReturnsAsync(Mock.Of<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>());
        _mockMapper.Setup(x => x.Map<PropertyDto>(It.IsAny<Property>())).Returns(propertyDto);

        // Act
        var result = await _propertyUseCases.CreatePropertyAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Casa en Madrid");
    }

    [Test]
    public async Task UpdatePropertyPriceAsync_WithValidData_ShouldReturnTrue()
    {
        // Arrange
        var propertyId = 1;
        var updatePriceDto = new UpdatePropertyPriceDto { Price = 300000 };
        var property = new Property("Casa en Madrid", "Calle Mayor 123", 250000, "PROP001", 1);

        _mockPropertyRepository.Setup(x => x.GetByIdAsync(propertyId)).Returns(property);
        _mockPropertyRepository.Setup(x => x.UpdateAsync(It.IsAny<Property>())).ReturnsAsync(property);

        // Act
        var result = await _propertyUseCases.UpdatePropertyPriceAsync(propertyId, updatePriceDto);

        // Assert
        result.Should().BeTrue();
        property.Price.Should().Be(300000);
    }

    [Test]
    public async Task AddImagesToPropertyAsync_WithValidData_ShouldReturnTrue()
    {
        // Arrange
        var propertyId = 1;
        var imageBytes = new byte[] { 1, 2, 3, 4, 5 };
        var mockFormFile = new Mock<IFormFile>();
        mockFormFile.Setup(f => f.Length).Returns(1024); // Simular archivo con contenido
        var imageFiles = new List<IFormFile> { mockFormFile.Object };

        _mockPropertyRepository.Setup(x => x.ExistsAsync(propertyId)).ReturnsAsync(true);
        _mockFileService.Setup(x => x.ProcessImageAsync(It.IsAny<IFormFile>())).ReturnsAsync(imageBytes);
        _mockPropertyImageRepository.Setup(x => x.AddAsync(It.IsAny<PropertyImage>())).ReturnsAsync(new PropertyImage(1, imageBytes, true));

        // Act
        var result = await _propertyUseCases.AddImagesToPropertyAsync(propertyId, imageFiles);

        // Assert
        result.Should().BeTrue();
    }
}