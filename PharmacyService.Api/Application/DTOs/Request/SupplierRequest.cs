using VehicleType = PharmacyService.Api.Domain.Enums.VehicleType;

namespace PharmacyService.Api.Application.DTOs.Request;

public record SupplierRequest
{
    public required string Name { get; init; }
    public required string Address { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public VehicleType VehicleType { get; init; }
    public string VehicleNumber { get; set; } = string.Empty;
}