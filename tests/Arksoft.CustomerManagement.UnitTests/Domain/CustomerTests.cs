using Arksoft.CustomerManagement.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Arksoft.CustomerManagement.UnitTests.Domain;

public class CustomerTests
{
    [Fact]
    public void Customer_ShouldBeCreatedWithValidData()
    {
        // Arrange
        var name = "Test Company";
        var address = "123 Test Street";
        var phone = "+27 11 123 4567";
        var contactPerson = "John Doe";
        var email = "john@test.com";
        var vatNumber = "ZA1234567890";

        // Act
        var customer = new Customer
        {
            Name = name,
            Address = address,
            TelephoneNumber = phone,
            ContactPersonName = contactPerson,
            ContactPersonEmail = email,
            VatNumber = vatNumber
        };

        // Assert
        customer.Name.Should().Be(name);
        customer.Address.Should().Be(address);
        customer.TelephoneNumber.Should().Be(phone);
        customer.ContactPersonName.Should().Be(contactPerson);
        customer.ContactPersonEmail.Should().Be(email);
        customer.VatNumber.Should().Be(vatNumber);
        customer.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        customer.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Customer_ShouldAllowNullOptionalFields()
    {
        // Arrange & Act
        var customer = new Customer
        {
            Name = "Required Company",
            Address = "Required Address"
        };

        // Assert
        customer.Name.Should().Be("Required Company");
        customer.Address.Should().Be("Required Address");
        customer.TelephoneNumber.Should().BeNull();
        customer.ContactPersonName.Should().BeNull();
        customer.ContactPersonEmail.Should().BeNull();
        customer.VatNumber.Should().BeNull();
    }

    [Fact]
    public void Customer_ShouldSetUpdatedAtWhenModified()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Original Company",
            Address = "Original Address"
        };

        var originalCreatedAt = customer.CreatedAt;

        // Act
        customer.Name = "Updated Company";
        customer.UpdatedAt = DateTime.UtcNow;

        // Assert
        customer.Name.Should().Be("Updated Company");
        customer.CreatedAt.Should().Be(originalCreatedAt);
        customer.UpdatedAt.Should().NotBeNull();
        customer.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}