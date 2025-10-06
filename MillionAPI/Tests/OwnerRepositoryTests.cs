using NUnit.Framework;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MillionAPI.Infrastructure.Repositories;
using MillionAPI.Infrastructure.Data;
using MillionAPI.Domain.Entities;

namespace MillionAPI.Tests;

[TestFixture]
public class OwnerRepositoryTests
{
    private ContextDB _context;
    private OwnerRepository _repository;

    [SetUp]
    public void Setup()
    {
        // Configurar base de datos en memoria para pruebas
        var options = new DbContextOptionsBuilder<ContextDB>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ContextDB(options);
        _repository = new OwnerRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task AddAsync_WithValidOwner_ShouldAddOwnerToDatabase()
    {
        // Arrange
        var owner = new Owner("Juan Pérez", "Calle 123 #45-67");

        // Act
        var result = await _repository.AddAsync(owner);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("Juan Pérez");
        result.Address.Should().Be("Calle 123 #45-67");

        // Verificar que se guardó en la base de datos
        var savedOwner = await _context.Owner.FindAsync(result.Id);
        savedOwner.Should().NotBeNull();
        savedOwner!.Name.Should().Be("Juan Pérez");
    }

    [Test]
    public async Task GetAllAsync_WithMultipleOwners_ShouldReturnAllOwners()
    {
        // Arrange
        var owner1 = new Owner("Juan Pérez", "Calle 123");
        var owner2 = new Owner("María García", "Avenida 456");

        await _repository.AddAsync(owner1);
        await _repository.AddAsync(owner2);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(o => o.Name == "Juan Pérez");
        result.Should().Contain(o => o.Name == "María García");
    }

    [Test]
    public async Task ExistsAsync_WithExistingOwner_ShouldReturnTrue()
    {
        // Arrange
        var owner = new Owner("Juan Pérez", "Calle 123");
        var addedOwner = await _repository.AddAsync(owner);

        // Act
        var exists = await _repository.ExistsAsync(addedOwner.Id);

        // Assert
        exists.Should().BeTrue();
    }

    [Test]
    public async Task ExistsAsync_WithNonExistingOwner_ShouldReturnFalse()
    {
        // Act
        var exists = await _repository.ExistsAsync(999);

        // Assert
        exists.Should().BeFalse();
    }
}

