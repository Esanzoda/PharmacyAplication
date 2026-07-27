using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Purchase.Commands;

public record UpdatePurchaseCommand(
    long PharmacyId,
    long EmployeeId,
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

        purchase.EmployeeId = request.EmployeeId;
        mapper.Map(request.Request, purchase);
       // dbContext.Purchases.Update(purchase);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PurchaseResponse>(purchase);
    }
}