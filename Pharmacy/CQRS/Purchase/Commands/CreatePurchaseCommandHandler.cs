using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Purchase.Models;
using Pharmacy.CQRS.Purchase.Models.DTOs.Request;
using Pharmacy.CQRS.Purchase.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Commands;

public record CreatePurchaseCommand(
    long PharmacyId,
    PurchaseRequest Request
) : IRequest<PurchaseResponse>;

public class CreatePurchaseCommandHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<CreatePurchaseCommand, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = mapper.Map<Models.Purchase>(request.Request);
        purchase.PharmacyId = request.PharmacyId;
        await dbContext.Purchases
            .AddAsync(purchase, cancellationToken);
        var productIds = request.Request.PurchaseItems
            .Select(x => x.ProductId)
            .ToList();
        var products = await dbContext.Products
            .Where(x => productIds.Contains(x.Id) &&
                        x.PharmacyId == request.PharmacyId)
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (var item in request.Request.PurchaseItems)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
            {
                throw new RecourseNotFoundException("Product not found");
            }

            var purchaseItem = mapper.Map<PurchaseItem>(item);
            purchaseItem.PharmacyId = request.PharmacyId;
            purchaseItem.TotalPrice = item.Quantity * item.PurchasePrice;

            purchase.PurchaseItems.Add(purchaseItem);
            product.Stock += item.Quantity;
        }

        purchase.TotalAmount = purchase.PurchaseItems.Sum(x => x.TotalPrice);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PurchaseResponse>(purchase);
    }
}