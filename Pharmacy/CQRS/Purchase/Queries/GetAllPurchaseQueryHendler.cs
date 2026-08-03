using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Purchase.Models.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Queries;

public record GetAllPurchaseQuery(
    long PharmacyId,
    int Page,
    int PageSize) : IRequest<List<PurchaseResponse>>;

public class GetAllPurchaseQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetAllPurchaseQuery, List<PurchaseResponse>>
{
    public async Task<List<PurchaseResponse>> Handle(GetAllPurchaseQuery request, CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .Where(x => x.PharmacyId == request.PharmacyId)
            .Include(o => o.PurchaseItems)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return mapper.Map<List<PurchaseResponse>>(purchase);
    }
}