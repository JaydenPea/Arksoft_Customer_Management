using Arksoft.CustomerManagement.Application.Common.DTOs;

namespace Arksoft.CustomerManagement.Application.Common.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
    Task<CustomerDto> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto);
    Task<bool> DeleteCustomerAsync(int id);
    Task<CustomerDto?> GetCustomerByIdAsync(int id);
    Task<PagedResult<CustomerListDto>> GetCustomersAsync(int pageNumber, int pageSize, string? searchTerm = null, string? sortBy = null, bool sortDescending = false);
}