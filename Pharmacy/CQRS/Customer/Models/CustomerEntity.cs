using Pharmacy.CQRS.Cart.Models;
using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Customer.Models;

public class CustomerEntity : User
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public CartEntity CartEntity { get; set; } = null!;
}