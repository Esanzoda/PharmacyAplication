namespace Pharmacy.Event.Events;

public class OrderCancelledEvent
{
    public long OrderId { get; init; }
    public required string Email { get; init; }
    public long CustomerId { get; init; }
    public DateTime UpdateTime { get; init; }
}