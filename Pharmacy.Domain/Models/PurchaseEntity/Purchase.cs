using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Employee;

namespace Pharmacy.Domain.Models.PurchaseEntity;

public class Purchase : BaseEntity
{
    public long PharmacyId { get; set; }
    public decimal TotalAmount { get; set; }
    public long EmployeeEntityId { get; set; }
    public EmployeeEntity EmployeeEntity { get; set; } = null!;

    public List<PurchaseItem> PurchaseItems { get; set; } = [];
}