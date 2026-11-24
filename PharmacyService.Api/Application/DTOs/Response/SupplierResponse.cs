using PharmacyService.Domain.Entities;
using PharmacyService.Domain.Enums;

namespace PharmacyService.Api.Application.DTOs.Response;

public class SupplierResponse : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public VehicleType VehicleType { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
}