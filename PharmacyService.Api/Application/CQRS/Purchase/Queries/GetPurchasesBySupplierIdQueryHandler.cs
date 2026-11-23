using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Purchase.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;

namespace PharmacyService.Api.Application.CQRS.Purchase.Queries;

public record GetPurchasesBySupplierIdQuery(
    long SupplierId,
    long PharmacyId,
    PaginationRequest PaginationRequest) : IRequest<List<PurchaseResponse>>;

public class GetPurchasesBySupplierIdQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetPurchasesBySupplierIdQuery, List<PurchaseResponse>>
{
    public async Task<List<PurchaseResponse>> Handle(GetPurchasesBySupplierIdQuery request,
        CancellationToken cancellationToken)
    {
        var purchases = await dbContext.Purchases
            .AsNoTracking()
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.SupplierId == request.SupplierId)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return PurchaseMappers.ToListPurchaseResponse(purchases);
    }
}