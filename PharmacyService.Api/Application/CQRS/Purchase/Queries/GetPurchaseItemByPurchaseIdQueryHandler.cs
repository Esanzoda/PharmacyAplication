using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Purchase.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;

namespace PharmacyService.Api.Application.CQRS.Purchase.Queries;

public record GetPurchaseItemByPurchaseIdQuery(
    long PurchaseId,
    long PharmacyId,
    PaginationRequest PaginationRequest) : IRequest<List<PurchaseItemResponse>>;

public class GetPurchaseItemByPurchaseIdQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetPurchaseItemByPurchaseIdQuery, List<PurchaseItemResponse>>
{
    public async Task<List<PurchaseItemResponse>> Handle(GetPurchaseItemByPurchaseIdQuery request,
        CancellationToken cancellationToken)
    {
        var purchaseItems = await dbContext.PurchaseItems
            .AsNoTracking()
            .Where(x => x.PurchaseEntityId == request.PurchaseId &&
                        x.PharmacyId == request.PharmacyId
            )
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return PurchaseMappers.ToPurchaseResponse(purchaseItems);
    }
}