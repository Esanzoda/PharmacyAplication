using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Product.ProductModels.DTos.Response;

public class ExpireDateProductResponse
{
    public decimal TotalSalePrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ExpireDateItemsResponse> ExpiryDateItemsListResponse { get; set; }
}

public record ExpireDateItemsResponse
{
    public long ExpiryDateEntityId { get; set; }
    public long ProductBatchId { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public decimal TotalSalePrice { get; set; }
}