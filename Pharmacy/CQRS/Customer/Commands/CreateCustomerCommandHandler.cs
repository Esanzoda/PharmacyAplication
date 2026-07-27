using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Auth.Commands;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Customer.Commands;

public record CreateCustomerCommand(
    CustomerRequest Request) : IRequest<CustomerResponse>;

public class CreateCustomerHandler(
    IMapper mapper,
    IApplicationDbContext dbContext,
    IMediator mediator)
    : IRequestHandler<CreateCustomerCommand, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
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

        var passwordHash = await mediator.Send(new PasswordHashCommand(request.Request.Password), cancellationToken);
        var newCustomer = mapper.Map<Models.Domain.Customer>(request.Request);
        newCustomer.PasswordHash = passwordHash;


        var cart = new Models.Domain.Cart
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