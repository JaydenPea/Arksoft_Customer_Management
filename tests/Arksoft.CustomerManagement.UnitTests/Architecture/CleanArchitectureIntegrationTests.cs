using Arksoft.CustomerManagement.Application.Common.Interfaces;
using Arksoft.CustomerManagement.Domain.Entities;
using Arksoft.CustomerManagement.Infrastructure.Data;
using Arksoft.CustomerManagement.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Arksoft.CustomerManagement.UnitTests.Architecture;

public class CleanArchitectureIntegrationTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly ICustomerRepository _repository;

    public CleanArchitectureIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new CustomerRepository(_context);
    }

    [Fact]
    public async Task Repository_ShouldCreateAndRetrieveCustomer()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Test Company",
            Address = "123 Test Street",
            VatNumber = "ZA1234567890"
        };

        // Act
        var createdCustomer = await _repository.AddAsync(customer);
        var retrievedCustomer = await _repository.GetByIdAsync(createdCustomer.Id);

        // Assert
        createdCustomer.Should().NotBeNull();
        createdCustomer.Id.Should().BeGreaterThan(0);
        retrievedCustomer.Should().NotBeNull();
        retrievedCustomer!.Name.Should().Be("Test Company");
        retrievedCustomer.VatNumber.Should().Be("ZA1234567890");
    }

    [Fact]
    public async Task Repository_ShouldUpdateCustomer()
    {
        // Arrange
        var customer = new Customer { Name = "Original Name", Address = "Original Address" };
        var createdCustomer = await _repository.AddAsync(customer);

        // Act
        createdCustomer.Name = "Updated Name";
        createdCustomer.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(createdCustomer);
        var updatedCustomer = await _repository.GetByIdAsync(createdCustomer.Id);

        // Assert
        updatedCustomer.Should().NotBeNull();
        updatedCustomer!.Name.Should().Be("Updated Name");
        updatedCustomer.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Repository_ShouldDeleteCustomer()
    {
        // Arrange
        var customer = new Customer { Name = "To Delete", Address = "123 Street" };
        var createdCustomer = await _repository.AddAsync(customer);

        // Act
        await _repository.DeleteAsync(createdCustomer);
        var deletedCustomer = await _repository.GetByIdAsync(createdCustomer.Id);

        // Assert
        deletedCustomer.Should().BeNull();
    }

    [Fact]
    public async Task Repository_ShouldReturnPagedResults()
    {
        // Arrange
        for (int i = 0; i < 5; i++)
        {
            await _repository.AddAsync(new Customer { Name = $"Company {i}", Address = $"{i} Street" });
        }

        // Act
        var results = await _repository.GetPagedAsync(1, 3);
        var totalCount = await _repository.GetTotalCountAsync();

        // Assert
        results.Should().HaveCount(3);
        totalCount.Should().Be(5);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}