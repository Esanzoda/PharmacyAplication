namespace Pharmacy.Models.Dto.Request;

public record OrderReservationRequest
{
    public long CustomerId { get; init; }
    
    public List<OrderItemRequest> OrderItemRequests { get; set; }
        = new();
}