using NUnit.Framework;
using FluentAssertions;
using Moq;
using MillionAPI.Application.UseCases;
using MillionAPI.Application.DTOs;
using MillionAPI.Domain.Entities;
using MillionAPI.Domain.Interfaces;
using MillionAPI.Application.Services;
using AutoMapper;

namespace MillionAPI.Tests;

[TestFixture]
public class OwnerUseCasesTests
{
    private Mock<IOwnerRepository> _mockOwnerRepository;
    private Mock<IMapper> _mockMapper;
    private Mock<IFileService> _mockFileService;
    private OwnerUseCases _ownerUseCases;

    [SetUp]
    public void Setup()
    {
        _mockOwnerRepository = new Mock<IOwnerRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockFileService = new Mock<IFileService>();
        _ownerUseCases = new OwnerUseCases(_mockOwnerRepository.Object, _mockMapper.Object, _mockFileService.Object);
    }

    [Test]
    public async Task CreateOwnerAsync_WithValidData_ShouldReturnOwnerDto()
    {
        // Arrange
        var createDto = new CreateOwnerDto
        {
            Name = "Juan Pérez",
            Address = "Calle 123 #45-67",
            Birthday = new DateOnly(1990, 5, 15)
        };

        var photoBytes = new byte[] { 1, 2, 3, 4, 5 };
        var owner = new Owner("Juan Pérez", "Calle 123 #45-67", photoBytes, new DateOnly(1990, 5, 15));
        var ownerDto = new OwnerDto
        {
            Id = 1,
            Name = "Juan Pérez",
            Address = "Calle 123 #45-67",
            Photo = photoBytes,
            Birthday = new DateOnly(1990, 5, 15),
            PropertiesCount = 0
        };

        _mockFileService.Setup(x => x.ProcessImageAsync(It.IsAny<IFormFile>()))
                       .ReturnsAsync(photoBytes);
        _mockOwnerRepository.Setup(x => x.AddAsync(It.IsAny<Owner>()))
                           .ReturnsAsync(owner);
        _mockMapper.Setup(x => x.Map<OwnerDto>(It.IsAny<Owner>()))
                  .Returns(ownerDto);

        // Act
        var result = await _ownerUseCases.CreateOwnerAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Juan Pérez");
        result.Address.Should().Be("Calle 123 #45-67");
        result.Photo.Should().BeEquivalentTo(photoBytes);
        result.Birthday.Should().Be(new DateOnly(1990, 5, 15));

        _mockFileService.Verify(x => x.ProcessImageAsync(It.IsAny<IFormFile>()), Times.Once);
        _mockOwnerRepository.Verify(x => x.AddAsync(It.IsAny<Owner>()), Times.Once);
        _mockMapper.Verify(x => x.Map<OwnerDto>(It.IsAny<Owner>()), Times.Once);
    }

    [Test]
    public async Task GetAllOwnersAsync_ShouldReturnListOfOwners()
    {
        // Arrange
        var owners = new List<Owner>
        {
            new Owner("Juan Pérez", "Calle 123", null, null),
            new Owner("María García", "Avenida 456", null, null)
        };

        var ownerDtos = new List<OwnerDto>
        {
            new OwnerDto { Id = 1, Name = "Juan Pérez", Address = "Calle 123", PropertiesCount = 0 },
            new OwnerDto { Id = 2, Name = "María García", Address = "Avenida 456", PropertiesCount = 0 }
        };

        _mockOwnerRepository.Setup(x => x.GetAllAsync())
                           .ReturnsAsync(owners);
        _mockMapper.Setup(x => x.Map<IEnumerable<OwnerDto>>(owners))
                  .Returns(ownerDtos);

        // Act
        var result = await _ownerUseCases.GetAllOwnersAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Juan Pérez");
        result.Last().Name.Should().Be("María García");

        _mockOwnerRepository.Verify(x => x.GetAllAsync(), Times.Once);
        _mockMapper.Verify(x => x.Map<IEnumerable<OwnerDto>>(owners), Times.Once);
    }
}

