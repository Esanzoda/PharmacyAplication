using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Deliver;

namespace Pharmacy.Domain.Models.Order.DTOs.Response;

public record OrderResponseForPharmacy
{
    public long Id { get; init; }
    public long CustomerId { get; init; }
    public long PharmacyId { get; init; }
    public OrderType OrderType { get; init; }
    public required string Address { get; init; }
    public DeliverEntity? Deliver { get; init; }
    public OrderStatus OrderStatus { get; init; }
    public DateTime? PicKupTime { get; init; }
    public decimal DeliveryFee { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<OrderItemResponse> OrderItemResponses { get; init; } = [];
}