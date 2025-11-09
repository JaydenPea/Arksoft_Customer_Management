using Arksoft.CustomerManagement.Application.Common.Interfaces;
using Arksoft.CustomerManagement.Domain.Entities;
using Arksoft.CustomerManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Arksoft.CustomerManagement.Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.ContactPersonEmail == email);
    }

    public async Task<Customer?> GetByVatNumberAsync(string vatNumber)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.VatNumber == vatNumber);
    }

    public async Task<IEnumerable<Customer>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, string? sortBy = null, bool sortDescending = false)
    {
        var query = _dbSet.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => 
                c.Name.Contains(searchTerm) || 
                (c.VatNumber != null && c.VatNumber.Contains(searchTerm)));
        }

        // Apply sorting
        query = sortBy?.ToLower() switch
        {
            "name" => sortDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
            "address" => sortDescending ? query.OrderByDescending(c => c.Address) : query.OrderBy(c => c.Address),
            "vatnumber" => sortDescending ? query.OrderByDescending(c => c.VatNumber) : query.OrderBy(c => c.VatNumber),
            "createdat" => sortDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
            _ => sortDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt)
        };

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? searchTerm = null)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => 
                c.Name.Contains(searchTerm) || 
                (c.VatNumber != null && c.VatNumber.Contains(searchTerm)));
        }

        return await query.CountAsync();
    }
}