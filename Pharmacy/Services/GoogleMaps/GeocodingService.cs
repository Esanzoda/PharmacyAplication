using System.Globalization;
using System.Text.Json;
using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Exception;

namespace Pharmacy.Services.GoogleMaps;

public interface IGeocodingService
{
    Task<Location> GetCoordinatesAsync(string address);
}

public class GeocodingService(HttpClient httpClient) : IGeocodingService
{
    public async Task<Location> GetCoordinatesAsync(string address)
    {
        var url = "https://nominatim.openstreetmap.org/search" +
                  $"?q={Uri.EscapeDataString(address)}&format=jsonv2&limit=1";
        using var req = new HttpRequestMessage(HttpMethod.Get, url);
        req.Headers.UserAgent.ParseAdd("PharmacyApplication/1.0 (your@email.com)");

        var response = await httpClient.SendAsync(req);
        response.EnsureSuccessStatusCode();

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        if (doc.RootElement.GetArrayLength() == 0)
            throw new ResourceNotFoundException("Address not found");

        var first = doc.RootElement[0];
        return new Location
        {
            Lat = decimal.Parse(first.GetProperty("lat").GetString()!, CultureInfo.InvariantCulture),
            Lng = decimal.Parse(first.GetProperty("lon").GetString()!, CultureInfo.InvariantCulture)
        };
    }
}