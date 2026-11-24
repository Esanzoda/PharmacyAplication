using PharmacyService.Domain.Enums;

namespace PharmacyService.Domain.Entities;

public class EmployeeEntity : User
{
    public long PharmacyId { get; set; }
    public decimal Salary { get; set; }
    public Position Position { get; set; }
}