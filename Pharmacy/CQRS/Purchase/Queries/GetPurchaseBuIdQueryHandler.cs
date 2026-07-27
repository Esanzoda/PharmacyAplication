using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Purchase.Queries;

public record GetPurchaseBuIdQuery(
    long PharmacyId,
    long Id) : IRequest<PurchaseResponse>;

public class GetPurchaseBuIdQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetPurchaseBuIdQuery, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(GetPurchaseBuIdQuery request, CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .Include(x => x.PurchaseItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Id,
                cancellationToken);
        if (purchase == null)
        {
            throw new RecourseNotFoundException("Purchase not found");
        }

        return mapper.Map<PurchaseResponse>(purchase);
    }
}