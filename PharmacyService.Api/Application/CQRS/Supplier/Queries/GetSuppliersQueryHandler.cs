using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Supplier.Mappers;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;

namespace PharmacyService.Api.Application.CQRS.Supplier.Queries;

public record GetSuppliersQuery(
    PaginationRequest PaginationRequest) : IRequest<List<SupplierResponse>>;

public class GetSuppliersQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetSuppliersQuery, List<SupplierResponse>>
{
    public async Task<List<SupplierResponse>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await dbContext.Suppliers
            .AsNoTracking()
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return SupplierMappers.ToSuppliersResponse(suppliers);
    }
}