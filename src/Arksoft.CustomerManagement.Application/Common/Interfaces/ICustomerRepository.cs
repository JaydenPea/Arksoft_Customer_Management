using Arksoft.CustomerManagement.Domain.Entities;

namespace Arksoft.CustomerManagement.Application.Common.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer?> GetByVatNumberAsync(string vatNumber);
    Task<IEnumerable<Customer>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, string? sortBy = null, bool sortDescending = false);
    Task<int> GetTotalCountAsync(string? searchTerm = null);
}