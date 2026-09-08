using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Base.Dto.Response;
using Pharmacy.Exception;
using Pharmacy.Infrastructure.Setting;
using Pharmacy.Interfaces;
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
    public async Task<LoginResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
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

        if (!await passwordService.PasswordVerify(request.Request.Password, user.PasswordHash))
        {
            throw new BusinessException("Invalid email or password");
        }

        var token = await authService.GenerateToken(user);

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
}