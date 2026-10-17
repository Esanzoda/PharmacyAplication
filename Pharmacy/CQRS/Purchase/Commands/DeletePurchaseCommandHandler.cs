using Grpc.Contract.Proto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Commands;

public record DeletePurchaseCommand(
    long PharmacyId,
    long EmployeeId,
    long Id) : IRequest<string>;

public class DeletePurchaseCommandHandler(
    IApplicationDbContext dbContext,
    ProductService.ProductServiceClient productClient) : IRequestHandler<DeletePurchaseCommand, string>
{
    public async Task<string> Handle(
        DeletePurchaseCommand request,
        CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .Include(x => x.PurchaseItems)
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Id && x.EmployeeEntityId == request.EmployeeId,
                cancellationToken);

        if (purchase is null)
        {
            throw new ResourceNotFoundException($"Purchase with id {request.Id} not found");
        }

        var purchaseItemIds = purchase.PurchaseItems
            .Select(x => x.Id)
            .ToList();

        var productBatch = await productClient.DeleteProductBatchesByPurchaseItemIdsAsync(
            new PurchaseItemIdsRequest
            {
                PurchaseItemId = { purchaseItemIds }
            },
            cancellationToken: cancellationToken);
        if (!productBatch.Success)
        {
            throw new BusinessException(productBatch.Message);
        }

        dbContext.Purchases
            .Remove(purchase);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Message.Deleted;
    }
}