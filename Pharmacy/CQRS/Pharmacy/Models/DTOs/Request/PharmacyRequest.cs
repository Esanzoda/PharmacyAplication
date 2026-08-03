namespace Pharmacy.CQRS.Pharmacy.Models.DTOs.Request;

public class PharmacyRequest
{
    public required string Name { get; init; }
    public required string Address { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Email { get; init; }
    public TimeOnly OpeningTime { get; init; }
    public TimeOnly ClosingTime { get; init; }
}