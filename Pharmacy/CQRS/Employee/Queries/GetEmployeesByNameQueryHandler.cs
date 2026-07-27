using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmployeesByNameQuery(
    long PharmacyId,
    string Name,
    int Page,
    int PageSize) : IRequest<List<EmployeeResponse>>;

public class GetEmployeesByNameQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetEmployeesByNameQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(GetEmployeesByNameQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Name.ToLower().Contains(request.Name.ToLower())
            )
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);


        if (!employees.Any())
        {
            throw new RecourseNotFoundException($"Employee with this name {request.Name} not found ");
        }

        return mapper.Map<List<EmployeeResponse>>(employees);
    }
}