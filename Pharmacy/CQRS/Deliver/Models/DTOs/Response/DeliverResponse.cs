namespace Pharmacy.CQRS.Deliver.Models.DTOs.Response;

public class PharmacyResponse
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public TimeOnly OpeningTime { get; set; }
    public TimeOnly ClosingTime { get; set; }
}