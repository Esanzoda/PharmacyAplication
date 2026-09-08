using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Customer;

namespace Pharmacy.Domain.Models.Cart;

public class CartEntity : BaseEntity
{
    public long CustomerEntityId { get; set; }
    public CustomerEntity CustomerEntity { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public List<CartItemEntity> CartItems { get; set; } = [];
}