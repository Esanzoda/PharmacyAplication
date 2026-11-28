namespace PharmacyService.Contract.Entities;

public class CartItemEntity : BaseEntity
{
    public long CustomerEntityId { get; set; }
    public long ProductEntityId { get; set; }
    public ProductEntity ProductEntity { get; set; } = null!;
    public CartEntity CartEntity { get; set; } = null!;
    public decimal SalePrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}