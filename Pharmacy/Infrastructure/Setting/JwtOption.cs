namespace Pharmacy.Infrastructure.Setting;

public class JwtOption
{
    public static string SettingName { get; set; } = "JwtSetting";
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpiryDay { get; set; }
}