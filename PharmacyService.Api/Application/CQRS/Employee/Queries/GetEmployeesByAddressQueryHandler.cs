using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Employee.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;

namespace PharmacyService.Api.Application.CQRS.Employee.Queries;

public record GetEmployeesByAddressQuery(
    long PharmacyId,
    string Address,
    PaginationRequest PaginationRequest) : IRequest<List<EmployeeResponse>>;

public class GetEmployeesByAddressQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetEmployeesByAddressQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(
        GetEmployeesByAddressQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Address.ToLower().Contains(request.Address.ToLower()) && x.IsDeleted == false)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return EmployeeMappers.ToListEmployeeResponse(employees);
    }
}