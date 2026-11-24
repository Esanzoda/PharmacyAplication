namespace PharmacyService.Domain.Entities;

public class Purchase : BaseEntity
{
    public long PharmacyId { get; set; }
    public decimal TotalAmount { get; set; }
    public long EmployeeEntityId { get; set; }
    public EmployeeEntity EmployeeEntity { get; set; } = null!;
    public long SupplierId { get; set; }
    public List<PurchaseItem> PurchaseItems { get; set; } = [];
}