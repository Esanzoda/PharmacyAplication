using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetAllEmployeeByPaginationQuery(
    long PharmacyId,
    int PageNumber,
    int PageSize) : IRequest<List<EmployeeResponse>>;

public class GetAllEmployeeByPaginationQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetAllEmployeeByPaginationQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(GetAllEmployeeByPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .Where(x=>x.PharmacyId==request.PharmacyId)
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return mapper.Map<List<EmployeeResponse>>(employees);
    }
}