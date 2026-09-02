using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Services.Password;

namespace Pharmacy.CQRS.Auth.Commands;

public record ResetPasswordCommand(
    int Code,
    string NewPassword,
    string Email,
    Role Role
) : IRequest<string>;

public class ResetPasswordCommandHandler(
    IApplicationDbContext dbContext,
    IDistributedCache cache,
    IPasswordService passwordService) : IRequestHandler<ResetPasswordCommand, string>
{
    public async Task<string> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var key = $"User-{request.Email}-{request.Role}";
        var userCode = await cache.GetStringAsync(key, cancellationToken);
        if (userCode is null)
        {
            throw new ResourceNotFoundException("Code expired ");
        }

        if (userCode != request.Code.ToString())
        {
            throw new BusinessException(" Invalid  code");
        }

        var user = await dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email &&
                                      x.Role == request.Role,
                cancellationToken);
        if (user is null)
        {
            throw new ResourceNotFoundException("User not found ");
        }

        user.PasswordHash = await passwordService.PasswordHash(request.NewPassword);
        await dbContext.SaveChangesAsync(cancellationToken);
        var response = $"Your password successful updated ";
        return response;
    }
}