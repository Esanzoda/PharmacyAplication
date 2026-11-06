using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Cart;

namespace Pharmacy.Domain.Models.Customer;

public class CustomerEntity : User
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public CartEntity CartEntity { get; set; } = null!;
}