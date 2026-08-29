using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
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

        var user = await dbContext.Users
            .FindAsync([refreshToken.UserId],
                cancellationToken);
        if (user is null)
        {
            throw new ResourceNotFoundException("User not found");
        }

        var newAccessToken = await authService.GenerateToken(user);
        return newAccessToken;
    }
}