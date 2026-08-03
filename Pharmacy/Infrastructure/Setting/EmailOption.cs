namespace Pharmacy.Infrastructure.Setting;

public class EmailOption
{
    public static string SettingName { get; set; } = "EmailSetting";
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string From { get; set; } = null!;
    public string FromName { get; set; } = null!;
}