using Arksoft.CustomerManagement.Domain.Common;

namespace Arksoft.CustomerManagement.Domain.Entities;

public class Customer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? TelephoneNumber { get; set; }
    public string? ContactPersonName { get; set; }
    public string? ContactPersonEmail { get; set; }
    public string? VatNumber { get; set; }
}