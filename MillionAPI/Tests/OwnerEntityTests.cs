using NUnit.Framework;
using FluentAssertions;
using MillionAPI.Domain.Entities;

namespace MillionAPI.Tests;

[TestFixture]
public class OwnerEntityTests
{
    [Test]
    public void Owner_Constructor_WithValidData_ShouldCreateOwner()
    {
        // Arrange
        var name = "Juan Pérez";
        var address = "Calle 123 #45-67";
        var photo = new byte[] { 1, 2, 3, 4, 5 };
        var birthday = new DateOnly(1990, 5, 15);

        // Act
        var owner = new Owner(name, address, photo, birthday);

        // Assert
        owner.Name.Should().Be(name);
        owner.Address.Should().Be(address);
        owner.Photo.Should().BeEquivalentTo(photo);
        owner.Birthday.Should().Be(birthday);
        owner.Properties.Should().NotBeNull();
        owner.Properties.Should().BeEmpty();
    }

    [Test]
    public void Owner_Constructor_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var address = "Calle 123 #45-67";

        // Act & Assert
        var action = () => new Owner(null!, address);
        action.Should().Throw<ArgumentNullException>()
              .WithParameterName("name");
    }

    [Test]
    public void Owner_Constructor_WithNullAddress_ShouldThrowArgumentNullException()
    {
        // Arrange
        var name = "Juan Pérez";

        // Act & Assert
        var action = () => new Owner(name, null!);
        action.Should().Throw<ArgumentNullException>()
              .WithParameterName("address");
    }

    [Test]
    public void Owner_Constructor_WithPhoto_ShouldSetPhoto()
    {
        // Arrange
        var name = "Juan Pérez";
        var address = "Calle 123";
        var photo = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var owner = new Owner(name, address, photo);

        // Assert
        owner.Photo.Should().BeEquivalentTo(photo);
    }

    [Test]
    public void Owner_Constructor_WithBirthday_ShouldSetBirthday()
    {
        // Arrange
        var name = "Juan Pérez";
        var address = "Calle 123";
        var birthday = new DateOnly(1990, 5, 15);

        // Act
        var owner = new Owner(name, address, null, birthday);

        // Assert
        owner.Birthday.Should().Be(birthday);
    }
}
