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

public record LoginDeliverCommand(
    LoginRequest Request) : IRequest<LoginResponse>;

public class LoginDeliverCommandHandler(
    IOptionsMonitor<JwtOption> jwt,
    IAuthService auth,
    IApplicationDbContext dbContext,
    IPasswordService passwordService) : IRequestHandler<LoginDeliverCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(
        LoginDeliverCommand request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var deliver = await dbContext.Delivers
            .FirstOrDefaultAsync(x => x.Email == request.Request.Email,
                cancellationToken);
        if (deliver is null)
        {
            throw new ResourceNotFoundException("Customer not found");
        }

        var passwordCheck = await passwordService.PasswordVerify(request.Request.Password, deliver.PasswordHash);
        if (!passwordCheck)
        {
            throw new BusinessException("Invalid email or password");
        }

        var accessToken = await auth.GenerateTokenForDeliver(deliver);
        var newRefreshToken =
            new RefreshToken
            {
                UserId = deliver.Id,
                Role = Role.Deliver,
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