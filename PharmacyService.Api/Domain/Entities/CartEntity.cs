namespace PharmacyService.Api.Domain.Entities;

public class CartEntity : BaseEntity
{
    public long CustomerEntityId { get; set; }
    public CustomerEntity CustomerEntity { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public List<CartItemEntity> CartItems { get; set; } = [];
}