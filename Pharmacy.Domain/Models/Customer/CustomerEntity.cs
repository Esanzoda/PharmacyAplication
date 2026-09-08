using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Cart;

namespace Pharmacy.Domain.Models.Customer;

public class CustomerEntity : User
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public CartEntity CartEntity { get; set; } = null!;
}