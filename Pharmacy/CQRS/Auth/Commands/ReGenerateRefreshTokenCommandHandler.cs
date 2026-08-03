using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Auth.Commands;

public record ReGenerateRefreshTokenCommand(
    string RefreshToken) : IRequest<string>;

public class ReGenerateRefreshTokenHandler(
    IApplicationDbContext dbContext,
    IMediator mediator)
    : IRequestHandler<ReGenerateRefreshTokenCommand, string>
{
    public async Task<string> Handle(ReGenerateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var refreshToken = await dbContext.RefreshTokens
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);
        if (refreshToken is null)
        {
            throw new RecourseNotFoundException("Invalid refresh token");
        }

        if (refreshToken.IsDeleted)
        {
            throw new RecourseNotFoundException("Refresh token not found or already deleted ");
        }

        if (refreshToken.ExpiresAt < now)
        {
            throw new BusinessException("Refresh token expired");
        }

        var newAccessToken = await mediator.Send(new GenerateTokenCommand(refreshToken.Customer), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return newAccessToken;
    }
}