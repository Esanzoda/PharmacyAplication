namespace PharmacyService.Contract.Entities;

public class CustomerEntity : User
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public CartEntity CartEntity { get; set; } = null!;
}