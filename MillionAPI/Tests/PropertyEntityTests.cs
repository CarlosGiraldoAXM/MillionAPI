using NUnit.Framework;
using FluentAssertions;
using MillionAPI.Domain.Entities;

namespace MillionAPI.Tests;

[TestFixture]
public class PropertyEntityTests
{
    [Test]
    public void Constructor_WithValidData_ShouldCreateProperty()
    {
        // Arrange & Act
        var property = new Property("Casa en Madrid", "Calle Mayor 123", 250000, "PROP001", 1, 2020);

        // Assert
        property.Name.Should().Be("Casa en Madrid");
        property.Address.Should().Be("Calle Mayor 123");
        property.Price.Should().Be(250000);
        property.CodeInternal.Should().Be("PROP001");
        property.OwnerId.Should().Be(1);
        property.Year.Should().Be(2020);
    }

    [Test]
    public void UpdatePrice_WithValidPrice_ShouldUpdatePrice()
    {
        // Arrange
        var property = new Property("Casa en Madrid", "Calle Mayor 123", 250000, "PROP001", 1, 2020);

        // Act
        property.UpdatePrice(300000);

        // Assert
        property.Price.Should().Be(300000);
    }

    [Test]
    public void Constructor_WithNullName_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action = () => new Property(null!, "Calle Mayor 123", 250000, "PROP001", 1, 2020);
        action.Should().Throw<ArgumentNullException>();
    }
}