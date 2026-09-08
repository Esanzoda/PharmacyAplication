using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.Employee.DTOs.Response;

public record EmployeeResponse
{
    public long Id { get; init; }
    public long PharmacyId { get; init; }
    public required string Name { get; init; }
    public required string Address { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public decimal Salary { get; init; }
    public Position Position { get; init; }
}