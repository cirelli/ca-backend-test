using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Domain.AutoMapperProfiles;

public class CustomerProfile
    : Profile
{
    public CustomerProfile()
    {
        CreateMap<CustomerDTO, Customer>();
        CreateMap<Customer, CustomerViewModel>();
    }
}
