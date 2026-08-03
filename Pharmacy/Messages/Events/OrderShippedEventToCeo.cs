namespace Pharmacy.Messages.Events;

public class OrderShippedEventToCeo
{
    public int Count { get; set; }
    public DateTime DateTime { get; set; }
}