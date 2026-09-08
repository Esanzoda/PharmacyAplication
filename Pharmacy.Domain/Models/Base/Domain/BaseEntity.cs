namespace Pharmacy.Domain.Models.Base.Domain;

public class BaseEntity
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public bool IsDeleted { get; set; }
}