using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels;
using Pharmacy.CQRS.Purchase.Mapper;
using Pharmacy.CQRS.Purchase.Models.DTOs.Request;
using Pharmacy.CQRS.Purchase.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Commands;

public record CreatePurchaseCommand(
    long PharmacyId,
    long EmployeeId,
    PurchaseRequest Request
) : IRequest<PurchaseResponse>;

public class CreatePurchaseCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<CreatePurchaseCommand, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(
        CreatePurchaseCommand request,
        CancellationToken cancellationToken)
    {
        var purchase = new Models.Purchase
        {
            PharmacyId = request.PharmacyId,
            EmployeeEntityId = request.EmployeeId
        };

        await dbContext.Purchases
            .AddAsync(purchase, cancellationToken);

        var productIds = request.Request.PurchaseItems
            .Select(x => x.ProductEntityId)
            .ToList();

        var products = await dbContext.Products
            .Where(x => productIds.Contains(x.Id) &&
                        x.PharmacyId == request.PharmacyId)
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (var item in request.Request.PurchaseItems)
        {
            if (!products.TryGetValue(item.ProductEntityId, out var product))
            {
                throw new ResourceNotFoundException("Product not found");
            }

            var purchaseItem = PurchaseMappers.ToPurchaseItem(item);
            purchaseItem.PharmacyId = request.PharmacyId;
            purchaseItem.ProductEntityId = product.Id;
            purchaseItem.PurchaseEntity = purchase;
            purchaseItem.TotalPrice = item.Quantity * item.PurchasePrice;

            purchase.PurchaseItems.Add(purchaseItem);
            var productBatch = new ProductBatch
            {
                PharmacyId = request.PharmacyId,
                Name = product.Name,
                ProductEntity = product,
                ProductEntityId = item.ProductEntityId,
                PurchaseItem = purchaseItem,
                Country = item.Country,
                Quantity = item.Quantity,
                ProductionDate = item.ProductionDate,
                ExpiryDate = item.ExpiryDate,
                PurchasePrice = item.PurchasePrice,
                TotalPurchasePrice = purchaseItem.TotalPrice,
                IsActive = true
            };
            product.Stock += item.Quantity;
            product.ProductBatches.Add(productBatch);
        }

        purchase.TotalAmount = purchase.PurchaseItems.Sum(x => x.TotalPrice);
        await dbContext.SaveChangesAsync(cancellationToken);

        return PurchaseMappers.ToPurchaseResponse(purchase);
    }
}