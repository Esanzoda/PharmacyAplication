using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Order.Models.DTOs.Response;

public class OrderResponseForCustomer
{
    public long Id { get; init; }
    public long PharmacyId { get; init; }
    public OrderStatus OrderStatus { get; init; }
    public OrderType OrderType { get; init; }
    public DateTime? PicKupTime { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Address { get; init; } = string.Empty;
    public Deliver.Models.Deliver? Deliver { get; init; }
    public decimal DeliveryFee { get; init; }
    public decimal TotalAmount { get; init; }
    public List<OrderItemResponse> OrderItemResponses { get; init; } 
}