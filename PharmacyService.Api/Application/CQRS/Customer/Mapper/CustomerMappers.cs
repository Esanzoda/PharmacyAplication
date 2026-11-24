using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Domain.Entities;
using PharmacyService.Domain.Enums;

namespace PharmacyService.Api.Application.CQRS.Customer.Mapper;

public static class CustomerMappers
{
    public static CustomerEntity ToCustomer(CustomerRequest request)
    {
        return new CustomerEntity
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            Role = Role.Customer
        };
    }

    public static void ToCustomer(
        CustomerEntity customer,
        UpdateCustomerRequest request)
    {
        customer.Name = request.Name;
        customer.Email = request.Email;
        customer.PhoneNumber = request.PhoneNumber;
    }

    public static CustomerResponse ToCustomerResponse(
        CustomerEntity customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Address = customer.Address,
            PhoneNumber = customer.PhoneNumber
        };
    }

    public static List<CustomerResponse> ToListCustomerResponse(
        List<CustomerEntity> customers)
    {
        return customers
            .Select(ToCustomerResponse)
            .ToList();
    }
}