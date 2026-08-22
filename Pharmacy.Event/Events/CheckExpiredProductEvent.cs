namespace Pharmacy.Event.Events;

public class CheckExpiredProductEvent
{
    public required string To { get; init; }
    public DateTime Day { get; init; }
    public int Count { get; init; }
    public decimal TotalPurchasePrice { get; init; }
    public decimal TotalSalePrice { get; init; }
    public List<ExpiredItemEvent> ExpiryDateItems { get; set; } = [];
}

public class ExpiredItemEvent
{
    public required string ProductName { get; init; }
    public long ProductBatchId { get; init; }
    public int Quantity { get; init; }
    public decimal TotalPurchasePrice { get; init; }
    public decimal TotalSalePrice { get; init; }
}