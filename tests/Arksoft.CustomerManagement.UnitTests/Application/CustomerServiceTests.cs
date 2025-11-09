using Arksoft.CustomerManagement.Application.Common.DTOs;
using Arksoft.CustomerManagement.Application.Common.Interfaces;
using Arksoft.CustomerManagement.Application.Services;
using Arksoft.CustomerManagement.Domain.Entities;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;

namespace Arksoft.CustomerManagement.UnitTests.Application;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CustomerService _customerService;

    public CustomerServiceTests()
    {
        _mockRepository = new Mock<ICustomerRepository>();
        _mockMapper = new Mock<IMapper>();
        _customerService = new CustomerService(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateCustomerAsync_ShouldReturnCustomerDto_WhenValidInput()
    {
        // Arrange
        var createDto = new CreateCustomerDto
        {
            Name = "Test Company",
            Address = "123 Test Street",
            TelephoneNumber = "+27 11 123 4567",
            ContactPersonName = "John Doe",
            ContactPersonEmail = "john@test.com",
            VatNumber = "ZA1234567890"
        };

        var customer = new Customer
        {
            Id = 1,
            Name = createDto.Name,
            Address = createDto.Address,
            TelephoneNumber = createDto.TelephoneNumber,
            ContactPersonName = createDto.ContactPersonName,
            ContactPersonEmail = createDto.ContactPersonEmail,
            VatNumber = createDto.VatNumber,
            CreatedAt = DateTime.UtcNow
        };

        var customerDto = new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address,
            TelephoneNumber = customer.TelephoneNumber,
            ContactPersonName = customer.ContactPersonName,
            ContactPersonEmail = customer.ContactPersonEmail,
            VatNumber = customer.VatNumber,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Customer>())).ReturnsAsync(customer);
        _mockMapper.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);

        // Act
        var result = await _customerService.CreateCustomerAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(createDto.Name);
        result.Address.Should().Be(createDto.Address);
        result.TelephoneNumber.Should().Be(createDto.TelephoneNumber);
        
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
        _mockMapper.Verify(m => m.Map<CustomerDto>(customer), Times.Once);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ShouldReturnCustomerDto_WhenCustomerExists()
    {
        // Arrange
        var customerId = 1;
        var customer = new Customer
        {
            Id = customerId,
            Name = "Test Company",
            Address = "123 Test Street",
            CreatedAt = DateTime.UtcNow
        };

        var customerDto = new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address,
            CreatedAt = customer.CreatedAt
        };

        _mockRepository.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);
        _mockMapper.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);

        // Act
        var result = await _customerService.GetCustomerByIdAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(customerId);
        result.Name.Should().Be(customer.Name);
        result.Address.Should().Be(customer.Address);
        
        _mockRepository.Verify(r => r.GetByIdAsync(customerId), Times.Once);
        _mockMapper.Verify(m => m.Map<CustomerDto>(customer), Times.Once);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = 999;
        _mockRepository.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync((Customer?)null);

        // Act
        var result = await _customerService.GetCustomerByIdAsync(customerId);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(customerId), Times.Once);
    }

    [Fact]
    public async Task UpdateCustomerAsync_ShouldReturnUpdatedCustomerDto_WhenCustomerExists()
    {
        // Arrange
        var customerId = 1;
        var updateDto = new UpdateCustomerDto
        {
            Name = "Updated Company",
            Address = "456 Updated Street",
            TelephoneNumber = "+27 11 987 6543"
        };

        var existingCustomer = new Customer
        {
            Id = customerId,
            Name = "Original Company",
            Address = "123 Original Street",
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        var updatedCustomer = new Customer
        {
            Id = customerId,
            Name = updateDto.Name,
            Address = updateDto.Address,
            TelephoneNumber = updateDto.TelephoneNumber,
            CreatedAt = existingCustomer.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        var customerDto = new CustomerDto
        {
            Id = updatedCustomer.Id,
            Name = updatedCustomer.Name,
            Address = updatedCustomer.Address,
            TelephoneNumber = updatedCustomer.TelephoneNumber,
            CreatedAt = updatedCustomer.CreatedAt,
            UpdatedAt = updatedCustomer.UpdatedAt
        };

        _mockRepository.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(existingCustomer);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(customerDto);

        // Act
        var result = await _customerService.UpdateCustomerAsync(customerId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(updateDto.Name);
        result.Address.Should().Be(updateDto.Address);
        result.TelephoneNumber.Should().Be(updateDto.TelephoneNumber);
        result.UpdatedAt.Should().NotBeNull();
        
        _mockRepository.Verify(r => r.GetByIdAsync(customerId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCustomerAsync_ShouldThrowKeyNotFoundException_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = 999;
        var updateDto = new UpdateCustomerDto
        {
            Name = "Updated Company",
            Address = "456 Updated Street"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync((Customer?)null);

        // Act & Assert
        await _customerService.Invoking(s => s.UpdateCustomerAsync(customerId, updateDto))
            .Should().ThrowAsync<KeyNotFoundException>();
        
        _mockRepository.Verify(r => r.GetByIdAsync(customerId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task DeleteCustomerAsync_ShouldReturnTrue_WhenCustomerExists()
    {
        // Arrange
        var customerId = 1;
        var customer = new Customer
        {
            Id = customerId,
            Name = "Test Company",
            Address = "123 Test Street"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);
        _mockRepository.Setup(r => r.DeleteAsync(customer)).Returns(Task.CompletedTask);

        // Act
        var result = await _customerService.DeleteCustomerAsync(customerId);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.GetByIdAsync(customerId), Times.Once);
        _mockRepository.Verify(r => r.DeleteAsync(customer), Times.Once);
    }

    [Fact]
    public async Task DeleteCustomerAsync_ShouldReturnFalse_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = 999;
        _mockRepository.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync((Customer?)null);

        // Act
        var result = await _customerService.DeleteCustomerAsync(customerId);

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(r => r.GetByIdAsync(customerId), Times.Once);
        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Customer>()), Times.Never);
    }
}