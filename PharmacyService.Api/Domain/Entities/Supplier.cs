using VehicleType = PharmacyService.Api.Domain.Enums.VehicleType;

namespace PharmacyService.Api.Domain.Entities;

public class Supplier : User
{
    public VehicleType VehicleType { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
}