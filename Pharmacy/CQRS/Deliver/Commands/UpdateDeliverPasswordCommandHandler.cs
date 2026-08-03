using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Services.Password;

namespace Pharmacy.CQRS.Deliver.Commands;

public record UpdateDeliverPasswordCommand(
    long Id,
    string Password,
    string NewPassword) : IRequest<string>;

public class UpdateDeliverPasswordCommandHandler(
    IDistributedCache cache,
    IApplicationDbContext dbContext,
    IPasswordService passwordService) : IRequestHandler<UpdateDeliverPasswordCommand, string>
{
    public async Task<string> Handle(UpdateDeliverPasswordCommand request, CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (deliver is null)
        {
            throw new RecourseNotFoundException("Deliver not found");
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

        var response = "Your password update successfully";
        return response;
    }
}