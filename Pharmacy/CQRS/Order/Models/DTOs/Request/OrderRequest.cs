using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Order.Models.DTOs.Request;

public record OrderRequest
{
    public OrderType OrderType { get; init; }
    public DateTime? PicKupTime { get; set; }

    public List<OrderItemRequest> OrderItems { get; init; }
}