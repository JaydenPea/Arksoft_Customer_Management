using Arksoft.CustomerManagement.Domain.Entities;
using Arksoft.CustomerManagement.Infrastructure.Data;
using Arksoft.CustomerManagement.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Arksoft.CustomerManagement.UnitTests.Infrastructure;

public class CustomerRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly CustomerRepository _repository;

    public CustomerRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new CustomerRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCustomerToDatabase()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Test Company",
            Address = "123 Test Street",
            TelephoneNumber = "+27 11 123 4567",
            ContactPersonName = "John Doe",
            ContactPersonEmail = "john@test.com",
            VatNumber = "ZA1234567890"
        };

        // Act
        var result = await _repository.AddAsync(customer);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be(customer.Name);
        result.Address.Should().Be(customer.Address);

        var savedCustomer = await _context.Customers.FindAsync(result.Id);
        savedCustomer.Should().NotBeNull();
        savedCustomer!.Name.Should().Be(customer.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Test Company",
            Address = "123 Test Street"
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(customer.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(customer.Id);
        result.Name.Should().Be(customer.Name);
        result.Address.Should().Be(customer.Address);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCustomerInDatabase()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Original Company",
            Address = "123 Original Street"
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        customer.Name = "Updated Company";
        customer.Address = "456 Updated Street";
        customer.UpdatedAt = DateTime.UtcNow;

        // Act
        await _repository.UpdateAsync(customer);
        var result = await _repository.GetByIdAsync(customer.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Updated Company");
        result.Address.Should().Be("456 Updated Street");
        result.UpdatedAt.Should().NotBeNull();

        var updatedCustomer = await _context.Customers.FindAsync(customer.Id);
        updatedCustomer!.Name.Should().Be("Updated Company");
        updatedCustomer.Address.Should().Be("456 Updated Street");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCustomerFromDatabase()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Test Company",
            Address = "123 Test Street"
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(customer);

        // Assert
        var deletedCustomer = await _context.Customers.FindAsync(customer.Id);
        deletedCustomer.Should().BeNull();
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnPagedResults()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new() { Name = "Alpha Company", Address = "123 Alpha Street" },
            new() { Name = "Beta Company", Address = "456 Beta Street" },
            new() { Name = "Gamma Company", Address = "789 Gamma Street" },
            new() { Name = "Delta Company", Address = "101 Delta Street" },
            new() { Name = "Echo Company", Address = "202 Echo Street" }
        };

        _context.Customers.AddRange(customers);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPagedAsync(2, 2); // Page 2, 2 items per page
        var totalCount = await _repository.GetTotalCountAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2); // Should return 2 items for page 2
        totalCount.Should().Be(5); // Should have 5 total items
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterBySearchTerm_WhenSearchTermProvided()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new() { Name = "Alpha Company", Address = "123 Street", VatNumber = "ZA1111" },
            new() { Name = "Beta Industries", Address = "456 Street", VatNumber = "ZA2222" },
            new() { Name = "Alpha Solutions", Address = "789 Street", VatNumber = "ZA3333" },
            new() { Name = "Gamma Corp", Address = "101 Street", VatNumber = "ZA1111" }
        };

        _context.Customers.AddRange(customers);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPagedAsync(1, 10, "Alpha");

        // Assert
        result.Should().HaveCount(2);
        result.All(c => c.Name.Contains("Alpha")).Should().BeTrue();
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByVatNumber_WhenVatSearchProvided()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new() { Name = "Company A", Address = "123 Street", VatNumber = "ZA1111" },
            new() { Name = "Company B", Address = "456 Street", VatNumber = "ZA2222" },
            new() { Name = "Company C", Address = "789 Street", VatNumber = "ZA1111" }
        };

        _context.Customers.AddRange(customers);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPagedAsync(1, 10, "ZA1111");

        // Assert
        result.Should().HaveCount(2);
        result.All(c => c.VatNumber == "ZA1111").Should().BeTrue();
    }

    [Theory]
    [InlineData("name", false)]
    [InlineData("name", true)]
    [InlineData("address", false)]
    [InlineData("createdat", true)]
    public async Task GetPagedAsync_ShouldSortCorrectly_BasedOnSortParameters(string sortBy, bool sortDescending)
    {
        // Arrange
        var customers = new List<Customer>
        {
            new() { Name = "Zebra Company", Address = "999 Zoo Street", CreatedAt = DateTime.UtcNow.AddDays(-3) },
            new() { Name = "Alpha Company", Address = "111 Alpha Street", CreatedAt = DateTime.UtcNow.AddDays(-1) },
            new() { Name = "Beta Company", Address = "555 Beta Street", CreatedAt = DateTime.UtcNow.AddDays(-2) }
        };

        _context.Customers.AddRange(customers);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPagedAsync(1, 10, null, sortBy, sortDescending);
        var resultList = result.ToList();

        // Assert
        resultList.Should().HaveCount(3);

        if (sortBy == "name")
        {
            if (sortDescending)
                resultList.First().Name.Should().Be("Zebra Company");
            else
                resultList.First().Name.Should().Be("Alpha Company");
        }
        else if (sortBy == "address")
        {
            if (sortDescending)
                resultList.First().Address.Should().Be("999 Zoo Street");
            else
                resultList.First().Address.Should().Be("111 Alpha Street");
        }
        else if (sortBy == "createdat")
        {
            if (sortDescending)
                resultList.First().CreatedAt.Should().BeAfter(resultList.Last().CreatedAt); // Most recent first
            else
                resultList.First().CreatedAt.Should().BeBefore(resultList.Last().CreatedAt); // Oldest first
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}