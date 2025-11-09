using Arksoft.CustomerManagement.Application.Common.DTOs;
using Arksoft.CustomerManagement.Domain.Entities;
using AutoMapper;

namespace Arksoft.CustomerManagement.Application.Common.Mappings;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<Customer, CustomerListDto>();
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();
    }
}