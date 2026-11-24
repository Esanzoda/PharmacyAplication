using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using PharmacyService.Api.Application.Exception;
using PharmacyService.Api.Services.Password;

namespace PharmacyService.Api.Application.CQRS.Deliver.Commands;

public record UpdateDeliverPasswordCommand(
    long Id,
    string Password,
    string NewPassword) : IRequest<string>;

public class UpdateDeliverPasswordCommandHandler(
    IDistributedCache cache,
    IApplicationDbContext dbContext,
    IPasswordService passwordService) : IRequestHandler<UpdateDeliverPasswordCommand, string>
{
    public async Task<string> Handle(
        UpdateDeliverPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FindAsync([request.Id],
                cancellationToken);

        if (deliver is null)
        {
            throw new ResourceNotFoundException("Deliver not found");
        }

        var passwordCheck = await passwordService.PasswordVerify(request.Password, deliver.PasswordHash);
        if (!passwordCheck)
        {
            throw new BusinessException("Invalid password");
        }

        deliver.PasswordHash = await passwordService.PasswordHash(request.NewPassword);

        await dbContext.SaveChangesAsync(cancellationToken);

        var key = $"DeliverById-{request.Id}";
        await cache.RemoveAsync(key, cancellationToken);

        var response = $"Your password update successfully {request.NewPassword}";
        
        return response;
    }
}