namespace Pharmacy.Event.Events;

public class OrderIsShippingEvent
{
    public required string Email { get; init; }
    public long OrderId { get; init; }
    public long CustomerId { get; init; }
    public decimal TotalAmount { get; init; }
    public decimal DeliveryFee { get; init; }
    public DateTime ShippedAt { get; init; }
    public required string DeliverName { get; init; }
}