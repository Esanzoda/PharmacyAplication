using MassTransit;
using PharmacyService.Api.Setting;

namespace PharmacyService.Api.Extensions;

public static class MassTransit
{
    public static void AddMassTransit(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMq = configuration
            .GetSection(RabbitMqOption.SettingName)
            .Get<RabbitMqOption>();

        if (rabbitMq is null)
            throw new InvalidOperationException(
                "RabbitMQ configuration is missing.");

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMq.Host, rabbitMq.VirtualHost, host =>
                {
                    host.Username(rabbitMq.UserName);
                    host.Password(rabbitMq.Password);
                });
            });
        });
    }
}