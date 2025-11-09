using Arksoft.CustomerManagement.Application.Common.DTOs;

namespace Arksoft.CustomerManagement.Web.Models;

public class CustomerListViewModel
{
    public PagedResult<CustomerListDto> Customers { get; set; } = new();
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class CustomerDetailsViewModel
{
    public CustomerDto Customer { get; set; } = new();
}

public class CreateCustomerViewModel
{
    public CreateCustomerDto Customer { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
}

public class EditCustomerViewModel
{
    public int CustomerId { get; set; }
    public UpdateCustomerDto Customer { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
}