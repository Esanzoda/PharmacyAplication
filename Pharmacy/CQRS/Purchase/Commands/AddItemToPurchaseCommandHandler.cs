using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Purchase.Models;
using Pharmacy.CQRS.Purchase.Models.DTOs.Request;
using Pharmacy.CQRS.Purchase.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Commands;

public record AddItemToPurchaseCommand(
    long PharmacyId,
    long Id,
    PurchaseItemRequest Request) : IRequest<PurchaseResponse>;

public class AddItemToPurchaseCommandHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<AddItemToPurchaseCommand, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(AddItemToPurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .Include(x => x.PurchaseItems)
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Id,
                cancellationToken);
        if (purchase is null)
        {
            throw new RecourseNotFoundException($"Purchase with this id not found");
        }

        var product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Request.ProductId,
                cancellationToken);
        if (product == null)
        {
            throw new RecourseNotFoundException($"Product not found");
        }

        var existItem = purchase.PurchaseItems
            .FirstOrDefault(x => x.ProductId == product.Id);
        if (existItem != null)
        {
            existItem.Quantity += request.Request.Quantity;
            existItem.TotalPrice = existItem.Quantity * request.Request.PurchasePrice;
        }
        else
        {
            var purchaseItem = mapper.Map<PurchaseItem>(request.Request);
            purchaseItem.PurchaseId = purchase.Id;
            purchaseItem.PharmacyId = request.PharmacyId;
            purchaseItem.TotalPrice = request.Request.Quantity * request.Request.PurchasePrice;

            purchase.PurchaseItems.Add(purchaseItem);
        }

        product.Stock += request.Request.Quantity;

        purchase.TotalAmount = purchase.PurchaseItems.Sum(item => item.TotalPrice);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<PurchaseResponse>(purchase);
    }
}