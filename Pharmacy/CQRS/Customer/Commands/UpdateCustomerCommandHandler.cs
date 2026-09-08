using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.CQRS.Customer.Mapper;
using Pharmacy.Domain.Models.Customer.DTOs.Request;
using Pharmacy.Domain.Models.Customer.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Customer.Commands;

public record UpdateCustomerCommand(
    long Id,
    UpdateCustomerRequest Request) : IRequest<CustomerResponse>;

public class UpdateCustomerCommandHandler(
    IDistributedCache cache,
    IApplicationDbContext dbContext) : IRequestHandler<UpdateCustomerCommand, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(
        UpdateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FindAsync([request.Id],
                cancellationToken);
        if (customer == null)
        {
            throw new ResourceNotFoundException($"Customer not found with id {request.Id}");
        }

        var customerExist = await dbContext.Customers
            .AnyAsync(
                x => x.Id != request.Id &&
                     (x.Email == request.Request.Email ||
                      x.PhoneNumber == request.Request.PhoneNumber),
                cancellationToken);

        if (customerExist)
        {
            throw new ResourceIsAlreadyExistException(
                $"Customer already exists with this phone number{request.Request.PhoneNumber} or with email{request.Request.Email} ");
        }


        CustomerMappers.ToCustomer(customer, request.Request);

        await dbContext.SaveChangesAsync(cancellationToken);

        var key = $"CustomerById-{customer.Id}";
        await cache.RemoveAsync(key, cancellationToken);

        return CustomerMappers.ToCustomerResponse(customer);
    }
}