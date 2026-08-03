using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Employee.Models.DTOs.Response;

public record EmployeeResponse
{
    public long PharmacyId { get; init; }
    public long Id { get; init; }
    public required string Name { get; init; }
    public required string Address { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public decimal Salary { get; init; }
    public Role Role { get; init; }
}