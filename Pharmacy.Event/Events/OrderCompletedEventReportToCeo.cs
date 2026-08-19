namespace Pharmacy.Event.Events;

public class OrderCompletedEventReportToCeo
{
    public required string To { get; init; }
    public DateTime Day { get; init; }
    public int Count { get; init; }
    public decimal TotalAmount { get; init; }
}