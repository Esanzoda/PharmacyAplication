using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.CQRS.Employee.Models.DTOs.Request;
using Pharmacy.CQRS.Employee.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Employee.Commands;

public record UpdateEmployeeCommand(
    long PharmacyId,
    long EmployeeId,
    UpdateEmployeeRequest Request) : IRequest<EmployeeResponse>;

public class UpdateEmployeeHandler(
    IApplicationDbContext dbContext,
    IDistributedCache cache,
    IMapper mapper) : IRequestHandler<UpdateEmployeeCommand, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.EmployeeId,
                cancellationToken);
        if (employee is null)
        {
            throw new RecourseNotFoundException($"Employee with id {request.EmployeeId} not found");
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
            throw new RecourseIsAlreadyExistException(
                $"Email: {request.Request.Email} or Number{request.Request.PhoneNumber}already exists");
        }


        mapper.Map(request.Request, employee);
        await dbContext.SaveChangesAsync(cancellationToken);

        var key = $"Employee-{request.PharmacyId}-{employee.Id}";
        await cache.RemoveAsync(key, cancellationToken);
        return mapper.Map<EmployeeResponse>(employee);
    }
}