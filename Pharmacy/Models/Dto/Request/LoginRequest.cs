using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Dto.Request;

public record LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public Role Role { get; set; }
}