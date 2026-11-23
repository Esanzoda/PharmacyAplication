using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PharmacyService.Api.Application.Exception;
using PharmacyService.Api.Services.Password;

namespace PharmacyService.Api.Application.CQRS.Employee.Commands;

public record UpdateEmployeePasswordCommand(
    long Id,
    long PharmacyId,
    string Password,
    string NewPassword) : IRequest<string>;

public class UpdateEmployeePasswordCommandHandler(
    IDistributedCache cache,
    IApplicationDbContext dbContext,
    IPasswordService passwordService) : IRequestHandler<UpdateEmployeePasswordCommand, string>
{
    public async Task<string> Handle(
        UpdateEmployeePasswordCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Id, cancellationToken);
        if (employee is null)
        {
            throw new ResourceNotFoundException("Employee not found");
        }

        var passwordCheck = await passwordService.PasswordVerify(request.Password, employee.PasswordHash);
        
        if (!passwordCheck)
        {
            throw new BusinessException("Invalid password");
        }

        employee.PasswordHash = await passwordService.PasswordHash(request.NewPassword);
        await dbContext.SaveChangesAsync(cancellationToken);

        var key = $"Employee-{request.PharmacyId}-{employee.Id}";
        await cache.RemoveAsync(key, cancellationToken);

        var response = $"Your password update successfully{request.NewPassword}";
        
        return response;
    }
}