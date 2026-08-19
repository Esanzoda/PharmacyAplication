namespace Notifications.Setting;

public class RabbitMqOption
{
    public static string SettingName { get; set; } = "Rabbitmq";
    public string Host { get; init; } = string.Empty;
    public string VirtualHost { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}