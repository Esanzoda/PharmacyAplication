using System.Text.Json;
using Pharmacy.Exception;
using Pharmacy.Infrastructure.Setting;
using Pharmacy.Models.Domain;

namespace Pharmacy.Services.GoogleMaps;

public interface IGeocodingService
{
    Task<Location> GetCoordinatesAsync(string address);
}

public class GeocodingService(
    HttpClient httpClient,
    IConfiguration configuration) : IGeocodingService
{
    public async Task<Location> GetCoordinatesAsync(string address)
    {
        var geocodingApiKet = configuration.GetSection(GoogleMap.SettingName)
            .Get<GoogleMap>();
        var url =
            $"https://maps.googleapis.com/maps/api/geocode/json" +
            $"?address={Uri.EscapeDataString(address)}" +
            $"&key={geocodingApiKet?.GeoCodingApiKey}";
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GoogleGeocodingResponse>(
            json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        if (result == null || result.Status != "OK")
        {
            throw new RecourseNotFoundException("Address not found");
        }

        return result.Results[0].Geometry.Location;
    }
}