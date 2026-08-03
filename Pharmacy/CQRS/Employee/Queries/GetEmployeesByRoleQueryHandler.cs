using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Employee.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmployeesByRoleQuery(
    long PharmacyId,
    Role Role,
    int Page,
    int PageSize) : IRequest<List<EmployeeResponse>>;

public class GetEmployeesByRoleQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetEmployeesByRoleQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(GetEmployeesByRoleQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Role == request.Role)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (employees.Count == 0)
        {
            throw new RecourseNotFoundException($"Employee with this role {request.Role} not found");
        }

        return mapper.Map<List<EmployeeResponse>>(employees);
    }
}