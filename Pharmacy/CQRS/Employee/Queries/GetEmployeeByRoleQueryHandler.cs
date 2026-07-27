using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmployeeByRoleQuery(
    long PharmacyId,
    Role Role,
    int Page,
    int PageSize) : IRequest<List<EmployeeResponse>>;

public class GetEmployeeByRoleQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetEmployeeByRoleQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(GetEmployeeByRoleQuery request,
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
        if (!employees.Any())
        {
            throw new RecourseNotFoundException($"Employee with this role {request.Role} not found");
        }

        return mapper.Map<List<EmployeeResponse>>(employees);
    }
}