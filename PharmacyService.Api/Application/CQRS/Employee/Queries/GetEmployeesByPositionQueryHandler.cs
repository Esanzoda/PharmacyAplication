using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Employee.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using Position = PharmacyService.Api.Domain.Enums.Position;

namespace PharmacyService.Api.Application.CQRS.Employee.Queries;

public record GetEmployeesByPositionQuery(
    long PharmacyId,
    Position Position,
    PaginationRequest PaginationRequest) : IRequest<List<EmployeeResponse>>;

public class GetEmployeesByPositionQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetEmployeesByPositionQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(
        GetEmployeesByPositionQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Position == request.Position &&
                        x.IsDeleted == false)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return EmployeeMappers.ToListEmployeeResponse(employees);
    }
}