namespace Pharmacy.Event.Events;

public class ForgotPasswordEvent
{
    public required string To { get; set; }
    public required string Message { get; set; }
    
}