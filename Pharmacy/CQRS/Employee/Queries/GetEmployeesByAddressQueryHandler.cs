using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Employee.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmployeesByAddressQuery(
    long PharmacyId,
    string Address,
    int Page,
    int PageSize) : IRequest<List<EmployeeResponse>>;

public class GetEmployeesByAddressQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetEmployeesByAddressQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(GetEmployeesByAddressQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Address.ToLower() == request.Address.ToLower())
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (employees.Count == 0)
        {
            throw new RecourseNotFoundException($"Employee with this address{request.Address} not found ");
        }

        return mapper.Map<List<EmployeeResponse>>(employees);
    }
}