using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Order.Models.DTOs.Response;

public record OrderResponse
{
    public long Id { get; init; }
    public long CustomerEntityId { get; init; }
    public long PharmacyId { get; init; }
    public OrderType OrderType { get; init; }
    public string Address { get; init; } = string.Empty;
    public Deliver.Models.DeliverEntity? Deliver { get; init; }
    public OrderStatus OrderStatus { get; init; }
    public DateTime PicKupTime { get; init; }
    public decimal DeliveryFee { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<OrderItemResponse> OrderItemResponses { get; init; }
}