using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.CQRS.Customer.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Services.Password;

namespace Pharmacy.CQRS.Customer.Commands;

public record UpdateCustomerPasswordCommand(
    long Id,
    string Password,
    string NewPassword) : IRequest<CustomerResponse>;

public class UpdateCustomerPasswordHandler(
    IDistributedCache cache,
    IApplicationDbContext dbContext,
    IMapper mapper,
    IPasswordService passwordService) : IRequestHandler<UpdateCustomerPasswordCommand, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(UpdateCustomerPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FindAsync(request.Id, cancellationToken);
        if (customer is null)
        {
            throw new RecourseNotFoundException("Customer not found");
        }

        var passwordCheck = await passwordService.PasswordVerify(request.Password, customer.PasswordHash);
        if (!passwordCheck)
        {
            throw new BusinessException("Invalid password");
        }

        customer.PasswordHash = await passwordService.PasswordHash(request.NewPassword);
        await dbContext.SaveChangesAsync(cancellationToken);

        var key = $"CustomerById-{customer.Id}";
        await cache.RemoveAsync(key, cancellationToken);

        return mapper.Map<CustomerResponse>(customer);
    }
}