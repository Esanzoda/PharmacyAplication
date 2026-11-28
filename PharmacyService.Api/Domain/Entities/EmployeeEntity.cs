using Position = PharmacyService.Api.Domain.Enums.Position;

namespace PharmacyService.Api.Domain.Entities;

public class EmployeeEntity : User
{
    public long PharmacyId { get; set; }
    public decimal Salary { get; set; }
    public Position Position { get; set; }
}