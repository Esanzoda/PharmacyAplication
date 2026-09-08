using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Employee.Mapper;
using Pharmacy.Domain.Models.Employee.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmployeeByEmailQuery(
    long PharmacyId,
    string Email) : IRequest<EmployeeResponse>;

public class GetEmployeeByEmailHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetEmployeeByEmailQuery, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(
        GetEmployeeByEmailQuery request,
        CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Email == request.Email,
                cancellationToken);

        return employee == null
            ? throw new ResourceNotFoundException($"Employee with this email {request.Email} not found ")
            : EmployeeMappers.ToEmployeeResponse(employee);
    }
}