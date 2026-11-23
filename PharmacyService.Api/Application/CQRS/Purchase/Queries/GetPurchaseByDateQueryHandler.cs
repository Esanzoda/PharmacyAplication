using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Purchase.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;

namespace PharmacyService.Api.Application.CQRS.Purchase.Queries;

public record GetPurchaseByDateQuery(
    long PharmacyId,
    DateOnly DateOnly,
    PaginationRequest PaginationRequest) : IRequest<List<PurchaseResponse>>;

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
                        x.CreatedAt.Date == request.DateOnly.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc).Date
            )
            .Include(o => o.PurchaseItems)
            .OrderBy(o => o.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return PurchaseMappers.ToListPurchaseResponse(purchases);
    }
}