namespace Pharmacy.Event.Events;

public class OrderCompletedEvent
{
    public long OrderId { get; init; }
    public long CustomerId { get; init; }
    public required string Email { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime CompletedAt { get; init; }
}