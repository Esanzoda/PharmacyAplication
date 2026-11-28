using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Domain.Entities;

namespace PharmacyService.Api.Application.CQRS.Purchase.Mapper;

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
            TotalPrice = purchaseItem.TotalPrice,
        };
    }

    public static List<PurchaseItemResponse> ToPurchaseResponse(List<PurchaseItem> purchaseItem)
    {
        var response = new List<PurchaseItemResponse>();
        response.AddRange(
            purchaseItem.Select(ToPurchaseItemResponse));
        return response;
    }

    public static PurchaseResponse ToPurchaseResponse(Domain.Entities.Purchase purchase)
    {
        return new PurchaseResponse
        {
            Id = purchase.Id,
            TotalAmount = purchase.TotalAmount,
            CreatedAt = purchase.CreatedAt,
            SupplierId = purchase.SupplierId,
            PurchaseItems = purchase.PurchaseItems
                .Select(ToPurchaseItemResponse)
                .ToList()
        };
    }

    public static List<PurchaseResponse> ToListPurchaseResponse(List<Domain.Entities.Purchase> purchases)
    {
        return purchases
            .Select(ToPurchaseResponse)
            .ToList();
    }
}