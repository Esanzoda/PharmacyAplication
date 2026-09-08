namespace Pharmacy.Domain.Models.Base.Domain.Enum;

public enum OrderStatus
{
    Pending = 0,
    ReadyForPickup = 1,
    Shipped = 2,
    Completed = 3,
    Cancelled = 4
}