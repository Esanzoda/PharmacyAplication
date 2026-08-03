using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Purchase.Models.DTOs.Request;
using Pharmacy.CQRS.Purchase.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Commands;

public record UpdatePurchaseCommand(
    long PharmacyId,
    long Id,
    PurchaseRequest Request) : IRequest<PurchaseResponse>;

public class UpdatePurchaseHandler(
    IApplicationDbContext dbContext,
    IMapper mapper)
    : IRequestHandler<UpdatePurchaseCommand, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(UpdatePurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Id,
                cancellationToken);
        if (purchase == null)
        {
            throw new RecourseNotFoundException("Purchase not found");
        }

        mapper.Map(request.Request, purchase);
        // dbContext.Purchases.Update(purchase);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PurchaseResponse>(purchase);
    }
}