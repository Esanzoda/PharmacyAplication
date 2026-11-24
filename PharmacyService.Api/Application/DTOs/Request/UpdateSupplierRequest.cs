using PharmacyService.Domain.Enums;

namespace PharmacyService.Api.Application.DTOs.Request;

public class UpdateSupplierRequest
{
    public required string Name { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Email { get; init; }
    public VehicleType VehicleType { get; init; }
    public required string VehicleNumber { get; set; }
}