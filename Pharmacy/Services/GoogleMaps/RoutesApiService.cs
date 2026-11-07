using System.Globalization;
using System.Text.Json;
using Pharmacy.Domain.Models.Base.Domain;

namespace Pharmacy.Services.GoogleMaps;

public interface IRoutesService
{
    Task<RoutesApiResponse> CalculateRouteAsync(RoutesApiRequest routesApiRequest);
}

public class RoutesApiService(HttpClient httpClient) : IRoutesService
{
    public async Task<RoutesApiResponse> CalculateRouteAsync(RoutesApiRequest routesApiRequest)
    {
        var url = string.Create(CultureInfo.InvariantCulture,
            $"http://router.project-osrm.org/route/v1/driving/" +
            $"{routesApiRequest.StartLng.ToString(CultureInfo.InvariantCulture)}," +
            $"{routesApiRequest.StartLat.ToString(CultureInfo.InvariantCulture)};" +
            $"{routesApiRequest.FinishLng.ToString(CultureInfo.InvariantCulture)}," +
            $"{routesApiRequest.FinishLat.ToString(CultureInfo.InvariantCulture)}?overview=false");

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.UserAgent.ParseAdd("PharmacyApplication");

        var response = await httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("routes", out var routes) || routes.GetArrayLength() == 0)
            throw new System.Exception($"Route not found.\nResponse:\n{json}");

        var route = routes[0];
        var meters = route.GetProperty("distance").GetDouble();
        var seconds = route.GetProperty("duration").GetDouble();
        return new RoutesApiResponse
        {
            DistanceKm = Math.Round(meters / 1000.0, 2),
            DurationMinutes = (int)Math.Ceiling(seconds / 60.0)
        };
    }
}