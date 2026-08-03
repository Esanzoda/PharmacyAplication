namespace Pharmacy.Models.Domain;

public class GoogleGeocodingResponse
{
    public string Status { get; set; } = "";

    public List<Result> Results { get; set; } = [];
}

public class Result
{
    public Geometry Geometry { get; set; } = new();
}

public class Geometry
{
    public Location Location { get; set; } = new();
}

public class Location
{
    public double Lat { get; set; }

    public double Lng { get; set; }
}