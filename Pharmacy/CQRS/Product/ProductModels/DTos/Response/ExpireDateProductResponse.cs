using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Product.ProductModels.DTos.Response;

public class ExpireDateProductResponse : BaseEntity
{
    public DateTime DateTime { get; set; }
    public int Count { get; set; }
    public decimal ToTalPrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public List<ExpireDateItemsResponse> ExpiryDateItemsListResponse { get; set; }
}

public record ExpireDateItemsResponse
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public required string ProductName { get; set; }
    public int Quantity { get; set; }
}