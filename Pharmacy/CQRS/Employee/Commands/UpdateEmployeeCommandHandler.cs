using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.CQRS.Employee.Mapper;
using Pharmacy.Domain.Models.Employee.DTOs.Request;
using Pharmacy.Domain.Models.Employee.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Employee.Commands;

public record UpdateEmployeeCommand(
    long PharmacyId,
    long EmployeeId,
    UpdateEmployeeRequest Request) : IRequest<EmployeeResponse>;

public class UpdateEmployeeHandler(
    IApplicationDbContext dbContext,
    IDistributedCache cache) : IRequestHandler<UpdateEmployeeCommand, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.EmployeeId,
                cancellationToken);

        if (employee is null)
        {
            throw new ResourceNotFoundException($"Employee with id {request.EmployeeId} not found");
        }

        var employeeExist = await dbContext.Employees
            .AnyAsync(x => x.PharmacyId == request.PharmacyId &&
                           x.Id != request.EmployeeId &&
                           (
                               x.Email == request.Request.Email ||
                               x.PhoneNumber == request.Request.PhoneNumber
                           ), cancellationToken);

        if (employeeExist)
        {
            throw new ResourceIsAlreadyExistException(
                $"Email: {request.Request.Email} or Number{request.Request.PhoneNumber} already exists");
        }

        EmployeeMappers.ToEmployee(employee, request.Request);
        await dbContext.SaveChangesAsync(cancellationToken);

        var key = $"Employee-{request.PharmacyId}-{employee.Id}";
        await cache.RemoveAsync(key, cancellationToken);
        return EmployeeMappers.ToEmployeeResponse(employee);
    }
}