using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Employee.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;

namespace PharmacyService.Api.Application.CQRS.Employee.Queries;

public record GetEmployeesByNameQuery(
    long PharmacyId,
    string Name,
    PaginationRequest PaginationRequest) : IRequest<List<EmployeeResponse>>;

public class GetEmployeesByNameQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetEmployeesByNameQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(
        GetEmployeesByNameQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Name.ToLower().Contains(request.Name.ToLower())
            )
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return EmployeeMappers.ToListEmployeeResponse(employees);
    }
}