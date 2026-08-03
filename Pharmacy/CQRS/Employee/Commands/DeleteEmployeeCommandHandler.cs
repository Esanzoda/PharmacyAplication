using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Employee.Commands;

public record DeleteEmployeeCommand(
    long PharmacyId,
    long EmployeeId) : IRequest<bool>;

public class DeleteEmployeeHandler(
    IDistributedCache cache,
    IApplicationDbContext dbContext) : IRequestHandler<DeleteEmployeeCommand, bool>
{
    public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .FirstOrDefaultAsync(
                x => x.PharmacyId == request.PharmacyId &&
                     x.Id == request.EmployeeId,
                cancellationToken);
        if (employee is null)
        {
            throw new RecourseNotFoundException($"Employee with id {request.EmployeeId} not found");
        }

        dbContext.Employees.Remove(employee);
        await dbContext.SaveChangesAsync(cancellationToken);

        var key = $"Employee-{request.PharmacyId}-{employee.Id}";
        await cache.RemoveAsync(key, cancellationToken);
        return true;
    }
}