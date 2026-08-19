using Pharmacy.CQRS.Employee.Models;
using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Purchase.Models;

public class Purchase : BaseEntity
{
    public long PharmacyId { get; set; }
    public decimal TotalAmount { get; set; }
    public long EmployeeEntityId { get; set; }
    public EmployeeEntity EmployeeEntity { get; set; } = null!;

    public List<PurchaseItem> PurchaseItems { get; set; } = new();
}