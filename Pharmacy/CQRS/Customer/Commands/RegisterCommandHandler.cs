using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Customer.Models.DTOs.Request;
using Pharmacy.CQRS.Customer.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Services.GoogleMaps;
using Pharmacy.Services.Password;

namespace Pharmacy.CQRS.Customer.Commands;

public record RegisterCommand(
    CustomerRequest Request) : IRequest<CustomerResponse>;

public class RegisterHandler(
    IMapper mapper,
    IApplicationDbContext dbContext,
    IGeocodingService geocodingService,
    IPasswordService passwordService) : IRequestHandler<RegisterCommand, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var customerExist = await dbContext.Customers
            .AnyAsync(x => x.Email == request.Request.Email ||
                           x.PhoneNumber == request.Request.PhoneNumber,
                cancellationToken);
        if (customerExist)
        {
            throw new RecourseIsAlreadyExistException(
                $"Customer already exists with this email {request.Request.Email} or number {request.Request.PhoneNumber}");
        }

        if (request.Request.Role is not Role.Customer)
        {
            throw new BusinessException("You can create only customer role.");
        }

        var passwordHash = await passwordService.PasswordHash(request.Request.Password);
        var geocoding = await geocodingService.GetCoordinatesAsync(request.Request.Address);
        var newCustomer = mapper.Map<Models.Customer>(request.Request);
        newCustomer.PasswordHash = passwordHash;
        newCustomer.Latitude = geocoding.Lat;
        newCustomer.Longitude = geocoding.Lng;


        var cart = new Cart.Models.Cart
        {
            Customer = newCustomer,
            TotalAmount = 0
        };
        await dbContext.Customers
            .AddAsync(newCustomer, cancellationToken);
        await dbContext.Carts
            .AddAsync(cart, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<CustomerResponse>(newCustomer);
    }
}