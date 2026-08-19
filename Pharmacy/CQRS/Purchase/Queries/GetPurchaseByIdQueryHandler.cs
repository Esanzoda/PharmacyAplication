using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Purchase.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Queries;

public record GetPurchaseBuIdQuery(
    long PharmacyId,
    long Id) : IRequest<PurchaseResponse>;

public class GetPurchaseByIdQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetPurchaseBuIdQuery, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(
        GetPurchaseBuIdQuery request,
        CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .Include(x => x.PurchaseItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Id,
                cancellationToken);

        return purchase == null
            ? throw new ResourceNotFoundException("Purchase not found")
            : mapper.Map<PurchaseResponse>(purchase);
    }
}