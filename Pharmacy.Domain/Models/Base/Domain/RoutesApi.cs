namespace Pharmacy.Domain.Models.Base.Domain;

public class RoutesApiRequest
{
    public decimal StartLat { get; init; }
    public decimal StartLng { get; init; }
    public decimal FinishLat { get; init; }
    public decimal FinishLng { get; init; }
}

public class RoutesApiResponse
{
    public double DistanceKm { get; init; }
    public int DurationMinutes { get; init; }
}