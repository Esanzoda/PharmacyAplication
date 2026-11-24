using PharmacyService.Domain.Enums;

namespace PharmacyService.Api.Application.DTOs.Request;

public record LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public Role Role { get; set; }
}