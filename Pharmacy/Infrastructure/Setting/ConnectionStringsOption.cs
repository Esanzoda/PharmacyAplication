namespace Pharmacy.Infrastructure.Setting;

public class ConnectionStringsOption
{
    public static string SettingName { get; set; } = "ConnectionStrings";
    public string DefaultConnection { get; init; } = string.Empty;
    public string Redis { get; init; } = string.Empty;
    public string InstanceName { get; init; } = string.Empty;
}