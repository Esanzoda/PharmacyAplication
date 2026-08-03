namespace Pharmacy.Infrastructure.Setting;

public class GoogleMap
{
    public static string SettingName { get; set; } = "GoogleMaps";
    public string GeoCodingApiKey { get; init; } = string.Empty;
    public string RoutesApiKey { get; init; } = string.Empty;
}