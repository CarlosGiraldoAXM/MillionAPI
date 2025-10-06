using NUnit.Framework;
using FluentAssertions;
using MillionAPI.Domain.Entities;
using MillionAPI.Infrastructure.Data;
using MillionAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MillionAPI.Tests;

[TestFixture]
public class PropertyRepositoryTests
{
    private ContextDB _context;
    private PropertyRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ContextDB>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ContextDB(options);
        _repository = new PropertyRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task AddAsync_WithValidProperty_ShouldAddProperty()
    {
        // Arrange
        var property = new Property("Casa en Madrid", "Calle Mayor 123", 250000, "PROP001", 1, 2020);

        // Act
        var result = await _repository.AddAsync(property);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Casa en Madrid");
        result.Price.Should().Be(250000);
    }

    [Test]
    public async Task GetAllAsync_WithProperties_ShouldReturnAllProperties()
    {
        // Arrange
        // Primero crear un Owner
        var owner = new Owner("Juan Pérez", "Calle Principal 123");
        _context.Owner.Add(owner);
        await _context.SaveChangesAsync();

        var property1 = new Property("Casa en Madrid", "Calle Mayor 123", 250000, "PROP001", owner.Id, 2020);
        var property2 = new Property("Casa en Barcelona", "Calle Gran Vía 456", 300000, "PROP002", owner.Id, 2021);
        
        await _repository.AddAsync(property1);
        await _repository.AddAsync(property2);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.Name == "Casa en Madrid");
        result.Should().Contain(p => p.Name == "Casa en Barcelona");
    }

    [Test]
    public void GetByIdAsync_WithExistingProperty_ShouldReturnProperty()
    {
        // Arrange
        var property = new Property("Casa en Madrid", "Calle Mayor 123", 250000, "PROP001", 1, 2020);
        _context.Property.Add(property);
        _context.SaveChanges();

        // Act
        var result = _repository.GetByIdAsync(property.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Casa en Madrid");
    }
}