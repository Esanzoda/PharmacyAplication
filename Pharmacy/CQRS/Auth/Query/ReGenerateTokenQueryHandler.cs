using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Services.Auth;

namespace Pharmacy.CQRS.Auth.Query;

public record ReGenerateTokenQuery(
    string RefreshToken) : IRequest<string>;

public class ReGenerateTokenQueryHandler(
    IAuthService authService,
    IApplicationDbContext dbContext) : IRequestHandler<ReGenerateTokenQuery, string>
{
    public async Task<string> Handle(
        ReGenerateTokenQuery request,
        CancellationToken cancellationToken)
    {
        var dateNow = DateTime.UtcNow;

        var refreshToken = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(
                x => x.Token == request.RefreshToken,
                cancellationToken);

        if (refreshToken is null)
        {
            throw new ResourceNotFoundException(
                "Invalid refresh token");
        }

        if (refreshToken.IsDeleted)
        {
            throw new ResourceNotFoundException(
                "Refresh token not found or already deleted");
        }

        if (refreshToken.ExpiresAt <= dateNow)
        {
            throw new BusinessException(
                "Refresh token expired");
        }

        var newAccessToken = refreshToken.Role switch
        {
            Role.Customer => await GenerateCustomerToken(
                refreshToken.UserId,
                cancellationToken),

            Role.Employee => await GenerateEmployeeToken(
                refreshToken.UserId,
                cancellationToken),

            Role.Deliver or Role.Admin => await GenerateDeliverOrCompanyEmployeeToken(
                refreshToken.UserId,
                cancellationToken),

            _ => throw new BusinessException(
                "Unknown role")
        };
        return newAccessToken;
    }

    private async Task<string> GenerateCustomerToken(
        long userId,
        CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken);

        if (customer is null)
        {
            throw new ResourceNotFoundException(
                "Customer not found");
        }

        return await authService.GenerateTokenForCustomer(customer);
    }

    private async Task<string> GenerateEmployeeToken(
        long userId, 
        CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken);

        if (employee is null)
        {
            throw new ResourceNotFoundException(
                "Employee not found");
        }

        return await authService.GenerateTokenForEmployee(employee);
    }

    private async Task<string> GenerateDeliverOrCompanyEmployeeToken(
        long userId, 
        CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken);

        if (user is null)
        {
            throw new ResourceNotFoundException(
                "Deliver not found");
        }

        return await authService.GenerateTokenForDeliverOrCompanyEmployee(user);
    }
}