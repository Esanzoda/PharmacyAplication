using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Models;
using Pharmacy.CQRS.Customer.Mapper;
using Pharmacy.CQRS.Customer.Models.DTOs.Request;
using Pharmacy.CQRS.Customer.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Services.GoogleMaps;
using Pharmacy.Services.Password;

namespace Pharmacy.CQRS.Customer.Commands;

public record RegisterCommand(
    CustomerRequest Request) : IRequest<CustomerResponse>;

public class RegisterHandler(
    IApplicationDbContext dbContext,
    IGeocodingService geocodingService,
    IPasswordService passwordService) : IRequestHandler<RegisterCommand, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var customerExist = await dbContext.Customers
            .AnyAsync(x => x.Email == request.Request.Email ||
                           x.PhoneNumber == request.Request.PhoneNumber,
                cancellationToken);
        if (customerExist)
        {
            throw new ResourceIsAlreadyExistException(
                $"Customer already exists with this email {request.Request.Email} or number {request.Request.PhoneNumber}");
        }

        var passwordHash = await passwordService.PasswordHash(request.Request.Password);

        var geocoding = await geocodingService.GetCoordinatesAsync(request.Request.Address);

        var newCustomer = CustomerMappers.ToCustomer(request.Request);
        newCustomer.PasswordHash = passwordHash;
        newCustomer.Latitude = geocoding.Lat;
        newCustomer.Longitude = geocoding.Lng;

        var cart = new CartEntity
        {
            CustomerEntity = newCustomer,
            TotalAmount = 0
        };
        await dbContext.Customers
            .AddAsync(newCustomer, cancellationToken);

        await dbContext.Carts
            .AddAsync(cart, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return CustomerMappers.ToCustomerResponse(newCustomer);
    }
}