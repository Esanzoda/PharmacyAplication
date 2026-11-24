using PharmacyService.Domain.Enums;

namespace PharmacyService.Domain.Entities;

public class Supplier : User
{
    public VehicleType VehicleType { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
}