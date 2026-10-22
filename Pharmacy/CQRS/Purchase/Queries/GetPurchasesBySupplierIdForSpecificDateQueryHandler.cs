using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Purchase.Mapper;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Queries;

public record GetPurchasesBySupplierIdForSpecificDateQuery(
    long SupplierId,
    long PharmacyId,
    DateOnly DateOnly,
    PaginationRequest PaginationRequest) : IRequest<List<PurchaseResponse>>;

public class GetPurchasesBySupplierIdForSpecificDateQueryHandler(
    IApplicationDbContext dbContext)
    : IRequestHandler<GetPurchasesBySupplierIdForSpecificDateQuery, List<PurchaseResponse>>
{
    public async Task<List<PurchaseResponse>> Handle(GetPurchasesBySupplierIdForSpecificDateQuery request,
        CancellationToken cancellationToken)
    {
        var time = TimeOnly.MaxValue;
        var purchases = await dbContext.Purchases
            .AsNoTracking()
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.SupplierId == request.SupplierId &&
                        x.CreatedAt.Date == request.DateOnly.ToDateTime(time, DateTimeKind.Utc).Date
            )
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return PurchaseMappers.ToListPurchaseResponse(purchases);
    }
}