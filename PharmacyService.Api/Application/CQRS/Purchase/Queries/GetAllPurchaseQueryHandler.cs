using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Purchase.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;

namespace PharmacyService.Api.Application.CQRS.Purchase.Queries;

public record GetAllPurchaseQuery(
    long PharmacyId,
    PaginationRequest PaginationRequest) : IRequest<List<PurchaseResponse>>;

public class GetAllPurchaseQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetAllPurchaseQuery, List<PurchaseResponse>>
{
    public async Task<List<PurchaseResponse>> Handle(
        GetAllPurchaseQuery request,
        CancellationToken cancellationToken)
    {
        var purchases = await dbContext.Purchases
            .AsNoTracking()
            .Where(x => x.PharmacyId == request.PharmacyId)
            .Include(o => o.PurchaseItems)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return PurchaseMappers.ToListPurchaseResponse(purchases);
    }
}