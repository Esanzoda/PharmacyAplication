using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmployeeBySalaryQuery(
    long PharmacyId,
    decimal Salary,
    int Page,
    int PageSize) : IRequest<List<EmployeeResponse>>;

public class GetEmployeeBySalaryQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetEmployeeBySalaryQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(GetEmployeeBySalaryQuery request,
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
        if (!employees.Any())
        {
            throw new RecourseNotFoundException($"Employee with this salary {request.Salary} not found");
        }

        return mapper.Map<List<EmployeeResponse>>(employees);
    }
}