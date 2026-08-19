using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Services.Auth;

namespace Pharmacy.CQRS.Auth.Query;

public record ReGenerateRefreshTokenQuery(
    string RefreshToken) : IRequest<string>;

public class ReGenerateTokenQuery(
    IAuthService authService,
    IApplicationDbContext dbContext) : IRequestHandler<ReGenerateRefreshTokenQuery, string>
{
    public async Task<string> Handle(ReGenerateRefreshTokenQuery request, CancellationToken cancellationToken)
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

            Role.Deliver => await GenerateDeliverToken(
                refreshToken.UserId,
                cancellationToken),

            Role.Admin => await GenerateCompanyEmployeeToken(
                refreshToken.UserId, cancellationToken),

            _ => throw new BusinessException(
                "Unknown role")
        };
        return newAccessToken;
    }

    private async Task<string> GenerateCustomerToken(long userId, CancellationToken cancellationToken)
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

    private async Task<string> GenerateEmployeeToken(long userId, CancellationToken cancellationToken)
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

    private async Task<string> GenerateDeliverToken(long userId, CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken);

        if (deliver is null)
        {
            throw new ResourceNotFoundException(
                "Deliver not found");
        }

        return await authService.GenerateTokenForDeliver(deliver);
    }

    private async Task<string> GenerateCompanyEmployeeToken(long userId, CancellationToken cancellationToken)
    {
        var companyEmployee = await dbContext.CompanyEmployees
            .FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken);
        if (companyEmployee is null)
        {
            throw new ResourceNotFoundException(
                "Deliver not found");
        }

        return await authService.GenerateTokenForCompanyEmployee(companyEmployee);
    }
}