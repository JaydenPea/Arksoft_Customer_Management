using Arksoft.CustomerManagement.Application.Common.DTOs;
using Arksoft.CustomerManagement.Application.Common.Interfaces;
using Arksoft.CustomerManagement.Domain.Entities;
using AutoMapper;

namespace Arksoft.CustomerManagement.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        var customer = new Customer
        {
            Name = createCustomerDto.Name.Trim(),
            Address = createCustomerDto.Address.Trim(),
            TelephoneNumber = createCustomerDto.TelephoneNumber?.Trim(),
            ContactPersonName = createCustomerDto.ContactPersonName?.Trim(),
            ContactPersonEmail = createCustomerDto.ContactPersonEmail?.Trim(),
            VatNumber = createCustomerDto.VatNumber?.Trim()?.ToUpperInvariant()
        };

        var createdCustomer = await _customerRepository.AddAsync(customer);
        return _mapper.Map<CustomerDto>(createdCustomer);
    }

    public async Task<CustomerDto> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto)
    {
        var existingCustomer = await _customerRepository.GetByIdAsync(id);
        if (existingCustomer == null)
            throw new KeyNotFoundException($"Customer with ID {id} not found");

        existingCustomer.Name = updateCustomerDto.Name.Trim();
        existingCustomer.Address = updateCustomerDto.Address.Trim();
        existingCustomer.TelephoneNumber = updateCustomerDto.TelephoneNumber?.Trim();
        existingCustomer.ContactPersonName = updateCustomerDto.ContactPersonName?.Trim();
        existingCustomer.ContactPersonEmail = updateCustomerDto.ContactPersonEmail?.Trim();
        existingCustomer.VatNumber = updateCustomerDto.VatNumber?.Trim()?.ToUpperInvariant();
        existingCustomer.UpdatedAt = DateTime.UtcNow;

        await _customerRepository.UpdateAsync(existingCustomer);
        return _mapper.Map<CustomerDto>(existingCustomer);
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            return false;

        await _customerRepository.DeleteAsync(customer);
        return true;
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        return customer == null ? null : _mapper.Map<CustomerDto>(customer);
    }

    public async Task<PagedResult<CustomerListDto>> GetCustomersAsync(int pageNumber, int pageSize, string? searchTerm = null, string? sortBy = null, bool sortDescending = false)
    {
        var customers = await _customerRepository.GetPagedAsync(pageNumber, pageSize, searchTerm, sortBy, sortDescending);
        var totalCount = await _customerRepository.GetTotalCountAsync(searchTerm);

        return new PagedResult<CustomerListDto>
        {
            Items = _mapper.Map<IEnumerable<CustomerListDto>>(customers),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}