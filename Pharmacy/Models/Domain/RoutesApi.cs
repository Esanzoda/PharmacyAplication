namespace Pharmacy.Models.Domain;

public class RoutesApiRequest
{
    public double StartLat { get; init; }

    public double StartLng { get; init; }

    public double FinishLat { get; init; }

    public double FinishLng { get; init; }
}

public class RoutesApiResponse
{
    public double DistanceKm { get; init; }

    public int DurationMinutes { get; init; }
}