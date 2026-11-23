using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Employee.Mapper;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;

namespace PharmacyService.Api.Application.CQRS.Employee.Queries;

public record GetEmployeeByNumberQuery(
    long PharmacyId,
    string Number) : IRequest<EmployeeResponse>;

public class GetEmployeeByNumberQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetEmployeeByNumberQuery, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(
        GetEmployeeByNumberQuery request,
        CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.PhoneNumber == request.Number,
                cancellationToken);

        return employee is null
            ? throw new ResourceNotFoundException($"Employee with this number {request.Number}  not found ")
            : EmployeeMappers.ToEmployeeResponse(employee);
    }
}