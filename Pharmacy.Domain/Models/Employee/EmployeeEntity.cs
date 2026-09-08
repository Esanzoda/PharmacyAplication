using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.Employee;

public class EmployeeEntity : User
{
    public long PharmacyId { get; set; }
    public decimal Salary { get; set; }
    public Position Position { get; set; }
}