using System.Globalization;
using System.Text;
using System.Text.Json;
using Pharmacy.Infrastructure.Setting;
using Pharmacy.Models.Domain;

namespace Pharmacy.Services.GoogleMaps;

public interface IRoutesService
{
    Task<RoutesApiResponse> CalculateRouteAsync(RoutesApiRequest request);
}

public class RoutesApiService(HttpClient httpClient, IConfiguration configuration) : IRoutesService
{
    public async Task<RoutesApiResponse> CalculateRouteAsync(RoutesApiRequest request)
    {
        var routesApiKey = configuration.GetSection(GoogleMap.SettingName)
            .Get<GoogleMap>();
        var url = "https://routes.googleapis.com/directions/v2:computeRoutes";
        var body = new
        {
            origin = new
            {
                location = new
                {
                    latLng = new
                    {
                        latitude = request.StartLat,
                        longitude = request.StartLng
                    }
                }
            },

            destination = new
            {
                location = new
                {
                    latLng = new
                    {
                        latitude = request.FinishLat,
                        longitude = request.FinishLng
                    }
                }
            },

            travelMode = "DRIVE"
        };

        var content = new StringContent(JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json");
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("X-Goog-Api-Key", routesApiKey?.RoutesApiKey);
        //httpClient.DefaultRequestHeaders.Remove("X-Goog-FieldMask");
        httpClient.DefaultRequestHeaders.Add("X-Goog-FieldMask", "routes.distanceMeters,routes.duration");


        var response = await httpClient.PostAsync(url, content);
        var result = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        using var doc = JsonDocument.Parse(result);
        try
        {
            if (!doc.RootElement.TryGetProperty("routes", out var routes))
            {
                throw new System.Exception($"Property 'routes' not found.\nResponse:\n{result}");
            }

            var route = routes[0];
            var meters = route.GetProperty("distanceMeters").GetDouble();
            var durationStr = route.GetProperty("duration").GetString() ?? "0s";
            double seconds = 0;
            if (double.TryParse(durationStr.TrimEnd('s'), CultureInfo.InvariantCulture, out double parsedSeconds))
            {
                seconds = parsedSeconds;
            }

            return new RoutesApiResponse
            {
                DistanceKm = Math.Round(meters / 1000.0, 2),
                DurationMinutes = (int)Math.Ceiling(seconds / 60.0)
            };
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}