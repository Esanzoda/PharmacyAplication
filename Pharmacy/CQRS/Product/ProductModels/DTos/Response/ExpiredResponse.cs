namespace Pharmacy.CQRS.Product.ProductModels.DTos.Response;

public class ExpiredResponse
{
    public decimal TotalSalePrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ExpiredItemsResponse> ExpiryDateItemsListResponse { get; set; } = [];
}

public record ExpiredItemsResponse
{
    public long ExpiryDateEntityId { get; set; }
    public long ProductBatchId { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public decimal TotalSalePrice { get; set; }
}