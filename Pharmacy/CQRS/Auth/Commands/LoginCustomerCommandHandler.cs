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

public record LoginCustomerCommand(
    LoginRequest Request) : IRequest<LoginResponse>;

public class LoginHandler(
    IOptionsMonitor<JwtOption> jwt,
    IAuthService auth,
    IApplicationDbContext dbContext,
    IPasswordService passwordService) : IRequestHandler<LoginCustomerCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(
        LoginCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(x => x.Email == request.Request.Email,
                cancellationToken);
        if (customer is null)
        {
            throw new ResourceNotFoundException("Customer not found");
        }

        var passwordCheck = await passwordService.PasswordVerify(request.Request.Password, customer.PasswordHash);
        if (!passwordCheck)
        {
            throw new BusinessException("Invalid email or password");
        }

        var accessToken = await auth.GenerateTokenForCustomer(customer);
        var newRefreshToken =
            new RefreshToken
            {
                UserId = customer.Id,
                Role = Role.Customer,
                Token = await auth.GenerateRefreshToken(),
                ExpiresAt = now.AddDays(jwt.CurrentValue.RefreshTokenExpiryDay),
                CreatedAt = now
            };

        await dbContext.RefreshTokens
            .AddAsync(newRefreshToken, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token
        };
    }
}