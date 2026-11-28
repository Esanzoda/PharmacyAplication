using PharmacyService.Api.Domain.Entities;
using PharmacyService.Api.Domain.Enums;

namespace PharmacyService.Api.Application.DTOs.Response;

public record OrderResponseForDeliver
{
    public long Id { get; init; }
    public long CustomerId { get; init; }
    public long PharmacyId { get; init; }
    public OrderType OrderType { get; init; }
    public required string Address { get; init; }
    public OrderStatus OrderStatus { get; init; }
    public decimal DeliveryFee { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime CreatedAt { get; init; }
    public required DeliverEntity DeliverEntity { get; set; }
}