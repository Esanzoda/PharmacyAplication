namespace Pharmacy.Event.Events;

public class OrderCreatedEvent
{
    public long OrderId { get; init; }
    public long CustomerId { get; init; }
    public DateTime CreatedAt { get; set; }
    public string? Address { get; init; }
    public decimal DeliveryFee { get; init; }
    public decimal TotalAmount { get; init; }
    public required string Email { get; init; }
}