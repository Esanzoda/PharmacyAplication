using Pharmacy.Domain.Models.Base.Domain;

namespace Pharmacy.Domain.Models.Deliver;

public class Supplier:User
{
    public VehicleType VehicleType { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
}