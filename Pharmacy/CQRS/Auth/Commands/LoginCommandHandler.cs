using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pharmacy.Exception;
using Pharmacy.Infrastructure.Setting;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;
using Pharmacy.Services.Auth;
using Pharmacy.Services.Password;

namespace Pharmacy.CQRS.Auth.Commands;

public record LoginCommand(
    LoginRequest Request) : IRequest<LoginResponse>;

public class LoginCommandHandler(
    IApplicationDbContext dbContext,
    IOptionsMonitor<JwtOption> jwt,
    IAuthService authService,
    IPasswordService passwordService) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var user = await dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == request.Request.Email &&
                                      x.Role == request.Request.Role,
                cancellationToken);
        if (user is null)
        {
            throw new ResourceNotFoundException("User not found");
        }

        if (await passwordService.PasswordVerify(request.Request.Password, user.PasswordHash))
        {
            throw new BusinessException("Invalid email or password");
        }

        var token = user.Role switch
        {
            Role.Customer => await GenerateTokenForCustomer(
                user.Id,
                cancellationToken),

            Role.Employee => await GenerateTokenForEmployee(
                user.Id,
                cancellationToken),

            Role.Deliver or Role.Admin => await GenerateTokenForDeliverOrCompanyEmployee(
                user.Id,
                cancellationToken),
            _ => throw new ArgumentOutOfRangeException()
        };
        var newRefreshToken =
            new RefreshToken
            {
                UserId = user.Id,
                Role = request.Request.Role,
                Token = await authService.GenerateRefreshToken(),
                ExpiresAt = now.AddDays(jwt.CurrentValue.RefreshTokenExpiryDay),
                CreatedAt = now
            };

        await dbContext.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new LoginResponse
        {
            AccessToken = token,
            RefreshToken = newRefreshToken.Token
        };
    }

    private async Task<string> GenerateTokenForCustomer(
        long id,
        CancellationToken ctx)
    {
        var customer = await dbContext.Customers
            .FindAsync([id],
                ctx);
        if (customer is null)
        {
            throw new ResourceNotFoundException("Customer not found");
        }

        return await authService.GenerateTokenForCustomer(customer);
    }

    private async Task<string> GenerateTokenForEmployee(
        long id,
        CancellationToken ctx)
    {
        var employee = await dbContext.Employees
            .FindAsync([id],
                ctx);
        if (employee is null)
        {
            throw new ResourceNotFoundException("Employee not found");
        }

        return await authService.GenerateTokenForEmployee(employee);
    }

    private async Task<string> GenerateTokenForDeliverOrCompanyEmployee(
        long id,
        CancellationToken ctx)
    {
        var user = await dbContext.Users
            .FindAsync([id],
                ctx);
        if (user is null)
        {
            throw new ResourceNotFoundException("Employee not found");
        }

        return await authService.GenerateTokenForDeliverOrCompanyEmployee(user);
    }
}