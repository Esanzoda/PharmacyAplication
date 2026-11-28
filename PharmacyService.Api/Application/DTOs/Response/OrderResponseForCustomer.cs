using PharmacyService.Api.Domain.Entities;
using PharmacyService.Api.Domain.Enums;

namespace PharmacyService.Api.Application.DTOs.Response;

public record OrderResponseForCustomer
{
    public long Id { get; init; }
    public long PharmacyId { get; init; }
    public OrderStatus OrderStatus { get; init; }
    public OrderType OrderType { get; init; }
    public DateTime CreatedAt { get; init; }
    public required string Address { get; init; }
    public DeliverEntity? Deliver { get; init; }
    public DateTime? PicKupTime { get; set; }
    public decimal DeliveryFee { get; init; }
    public decimal TotalAmount { get; init; }
    public List<OrderItemResponse> OrderItemResponses { get; init; } = [];
}