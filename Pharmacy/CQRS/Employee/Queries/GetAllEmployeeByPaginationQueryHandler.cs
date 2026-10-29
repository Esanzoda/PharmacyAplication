using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Employee.Mapper;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Employee.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetAllEmployeeByPaginationQuery(
    long PharmacyId,
    PaginationRequest PaginationRequest) : IRequest<List<EmployeeResponse>>;

public class GetAllEmployeeByPaginationQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetAllEmployeeByPaginationQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(GetAllEmployeeByPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .Where(x => x.PharmacyId == request.PharmacyId)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return EmployeeMappers.ToListEmployeeResponse(employees);
    }
}