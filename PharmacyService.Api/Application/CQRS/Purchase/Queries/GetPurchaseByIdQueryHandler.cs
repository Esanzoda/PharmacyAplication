using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Purchase.Mapper;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;

namespace PharmacyService.Api.Application.CQRS.Purchase.Queries;

public record GetPurchaseBuIdQuery(
    long PharmacyId,
    long Id) : IRequest<PurchaseResponse>;

public class GetPurchaseByIdQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetPurchaseBuIdQuery, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(
        GetPurchaseBuIdQuery request,
        CancellationToken cancellationToken)
    {
        var purchases = await dbContext.Purchases
            .Include(x => x.PurchaseItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Id,
                cancellationToken);

        return purchases == null
            ? throw new ResourceNotFoundException("Purchase not found")
            : PurchaseMappers.ToPurchaseResponse(purchases);
    }
}