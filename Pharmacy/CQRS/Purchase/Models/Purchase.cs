using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Purchase.Models;

public class Purchase : BaseEntity
{
    public long PharmacyId { get; set; }
    public decimal TotalAmount { get; set; }
    public long EmployeeId { get; set; }
    public Employee.Models.Employee Employee { get; set; }= null!;

    public List<PurchaseItem> PurchaseItems { get; set; }
        = new List<PurchaseItem>();
}