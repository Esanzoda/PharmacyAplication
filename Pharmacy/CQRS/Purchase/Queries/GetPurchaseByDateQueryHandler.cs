using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Purchase.Mapper;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Queries;

public record GetPurchaseByDateQuery(
    long PharmacyId,
    DateTime Date,
    int PageNumber,
    int PageSize) : IRequest<List<PurchaseResponse>>;

public class GetPurchaseByDateHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetPurchaseByDateQuery, List<PurchaseResponse>>
{
    public async Task<List<PurchaseResponse>> Handle(
        GetPurchaseByDateQuery request,
        CancellationToken cancellationToken)
    {
        var purchases = await dbContext.Purchases
            .AsNoTracking()
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.CreatedAt == request.Date)
            .Include(o => o.PurchaseItems)
            .OrderBy(o => o.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return PurchaseMappers.ToListPurchaseResponse(purchases);
    }
}