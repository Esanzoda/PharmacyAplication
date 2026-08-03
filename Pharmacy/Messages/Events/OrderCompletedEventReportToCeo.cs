namespace Pharmacy.Messages.Events;

public class OrderCompletedEventReportToCeo
{
    public required string To { get; set; }
    public DateTime Day { get; set; }
    public int Count { get; set; }
    public decimal TotalAmount { get; set; }
}