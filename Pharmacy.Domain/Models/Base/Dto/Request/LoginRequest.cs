using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.Base.Dto.Request;

public record LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public Role Role { get; set; }
}