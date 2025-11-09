using Arksoft.CustomerManagement.Application.Common.DTOs;
using Arksoft.CustomerManagement.Application.Validators;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Arksoft.CustomerManagement.UnitTests.Application;

public class CreateCustomerDtoValidatorTests
{
    private readonly CreateCustomerDtoValidator _validator;

    public CreateCustomerDtoValidatorTests()
    {
        _validator = new CreateCustomerDtoValidator();
    }

    [Fact]
    public void Validate_ShouldPass_WhenAllRequiredFieldsProvided()
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            Name = "Valid Company",
            Address = "123 Valid Street"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldFail_WhenNameIsEmpty()
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            Name = "",
            Address = "123 Valid Street"
        };

        // Act & Assert
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(dto.Name));
    }

    [Fact]
    public void Validate_ShouldFail_WhenAddressIsEmpty()
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            Name = "Valid Company",
            Address = ""
        };

        // Act & Assert
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(dto.Address));
    }

    [Fact]
    public void Validate_ShouldFail_WhenNameExceedsMaxLength()
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            Name = new string('A', 201), // 201 characters (max is 200)
            Address = "123 Valid Street"
        };

        // Act & Assert
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(dto.Name));
    }

    [Fact]
    public void Validate_ShouldFail_WhenAddressExceedsMaxLength()
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            Name = "Valid Company",
            Address = new string('A', 501) // 501 characters (max is 500)
        };

        // Act & Assert
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(dto.Address));
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@domain.com")]
    [InlineData("user@")]
    [InlineData("user.domain.com")]
    public void Validate_ShouldFail_WhenEmailIsInvalid(string invalidEmail)
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            Name = "Valid Company",
            Address = "123 Valid Street",
            ContactPersonEmail = invalidEmail
        };

        // Act & Assert
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(dto.ContactPersonEmail));
    }

    [Theory]
    [InlineData("user@domain.com")]
    [InlineData("test.email@company.co.za")]
    [InlineData("john.doe+test@example.org")]
    public void Validate_ShouldPass_WhenEmailIsValid(string validEmail)
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            Name = "Valid Company",
            Address = "123 Valid Street",
            ContactPersonEmail = validEmail
        };

        // Act & Assert
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldPass_WhenOptionalFieldsAreNull()
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            Name = "Valid Company",
            Address = "123 Valid Street",
            TelephoneNumber = null,
            ContactPersonName = null,
            ContactPersonEmail = null,
            VatNumber = null
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldFail_WhenContactPersonNameExceedsMaxLength()
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            Name = "Valid Company",
            Address = "123 Valid Street",
            ContactPersonName = new string('A', 101) // 101 characters (max is 100)
        };

        // Act & Assert
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(dto.ContactPersonName));
    }

    [Fact]
    public void Validate_ShouldFail_WhenTelephoneNumberExceedsMaxLength()
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            Name = "Valid Company",
            Address = "123 Valid Street",
            TelephoneNumber = new string('1', 21) // 21 characters (max is 20)
        };

        // Act & Assert
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(dto.TelephoneNumber));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVatNumberExceedsMaxLength()
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            Name = "Valid Company",
            Address = "123 Valid Street",
            VatNumber = new string('A', 51) // 51 characters (max is 50)
        };

        // Act & Assert
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(dto.VatNumber));
    }
}