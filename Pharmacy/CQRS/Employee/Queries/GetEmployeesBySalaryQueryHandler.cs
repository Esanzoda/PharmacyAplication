using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Employee.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmployeesBySalaryQuery(
    long PharmacyId,
    decimal Salary,
    int Page,
    int PageSize) : IRequest<List<EmployeeResponse>>;

public class GetEmployeesBySalaryQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetEmployeesBySalaryQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(GetEmployeesBySalaryQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Salary == request.Salary)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (employees.Count == 0)
        {
            throw new RecourseNotFoundException($"Employee with this salary {request.Salary} not found");
        }

        return mapper.Map<List<EmployeeResponse>>(employees);
    }
}