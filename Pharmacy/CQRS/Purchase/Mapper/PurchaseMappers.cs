using Pharmacy.CQRS.Purchase.Models;
using Pharmacy.CQRS.Purchase.Models.DTOs.Request;
using Pharmacy.CQRS.Purchase.Models.DTOs.Response;

namespace Pharmacy.CQRS.Purchase.Mapper;

public static class PurchaseMappers
{
    public static PurchaseItem ToPurchaseItem(PurchaseItemRequest request)
    {
        return new PurchaseItem
        {
            ProductEntityId = request.ProductEntityId,
            Quantity = request.Quantity,
            PurchasePrice = request.PurchasePrice,
            ExpiryDate = request.ExpiryDate,
            ProductionDate = request.ProductionDate,
            Country = request.Country
        };
    }

    private static PurchaseItemResponse ToPurchaseItemResponse(PurchaseItem purchaseItem)
    {
        return new PurchaseItemResponse
        {
            Id = purchaseItem.Id,
            PharmacyId = purchaseItem.PharmacyId,
            PurchaseEntityId = purchaseItem.PurchaseEntityId,
            ProductEntityId = purchaseItem.ProductEntityId,
            PurchasePrice = purchaseItem.PurchasePrice,
            Country = purchaseItem.Country,
            Quantity = purchaseItem.Quantity,
            Barcode = purchaseItem.Barcode,
            TotalPrice = purchaseItem.TotalPrice
        };
    }

    public static PurchaseResponse ToPurchaseResponse(Models.Purchase purchase)
    {
        return new PurchaseResponse
        {
            Id = purchase.Id,
            TotalAmount = purchase.TotalAmount,
            CreatedAt = purchase.CreatedAt,
            PurchaseItems = purchase.PurchaseItems
                .Select(ToPurchaseItemResponse)
                .ToList()
        };
    }

    public static List<PurchaseResponse> ToListPurchaseResponse(List<Models.Purchase> purchases)
    {
        return purchases
            .Select(ToPurchaseResponse)
            .ToList();
    }
}