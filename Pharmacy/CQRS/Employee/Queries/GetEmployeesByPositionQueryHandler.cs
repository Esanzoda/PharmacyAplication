using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Employee.Mapper;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Employee.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmployeesByPositionQuery(
    long PharmacyId,
    Position Position,
    int Page,
    int PageSize) : IRequest<List<EmployeeResponse>>;

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
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return EmployeeMappers.ToListEmployeeResponse(employees);
    }
}