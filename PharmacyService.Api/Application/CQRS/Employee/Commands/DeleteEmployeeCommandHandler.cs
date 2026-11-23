using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PharmacyService.Api.Application.Common;
using PharmacyService.Api.Application.Exception;

namespace PharmacyService.Api.Application.CQRS.Employee.Commands;

public record DeleteEmployeeCommand(
    long PharmacyId,
    long EmployeeId) : IRequest<string>;

public class DeleteEmployeeHandler(
    IDistributedCache cache,
    IApplicationDbContext dbContext) : IRequestHandler<DeleteEmployeeCommand, string>
{
    public async Task<string> Handle(
        DeleteEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .FirstOrDefaultAsync(
                x => x.PharmacyId == request.PharmacyId &&
                     x.Id == request.EmployeeId,
                cancellationToken);

        if (employee is null)
        {
            throw new ResourceNotFoundException($"Employee with id {request.EmployeeId} not found");
        }

        dbContext.Employees.Remove(employee);
        await dbContext.SaveChangesAsync(cancellationToken);

        var key = $"Employee-{request.PharmacyId}-{employee.Id}";
        await cache.RemoveAsync(key, cancellationToken);

        return Message.Deleted;
    }
}