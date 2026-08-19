namespace Pharmacy.Event.Events;

public class CheckExpiryDateProductEvent
{
    public required string To { get; init; }
    public DateTime Day { get; init; }
    public int Count { get; init; }
    public decimal TotalPurchasePrice { get; init; }
    public decimal TotalSalePrice { get; init; }
    public List<ExpiryDateItem> ExpiryDateItems { get; set; } = new();
}

public class ExpiryDateItem
{
    public required string ProductName { get; init; }
    public long ProductBatchId { get; init; }
    public int Quantity { get; init; }
    public decimal TotalPurchasePrice { get; init; }
    public decimal TotalSalePrice { get; init; }
}