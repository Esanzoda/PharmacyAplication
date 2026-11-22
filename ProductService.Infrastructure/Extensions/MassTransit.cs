using Infrastructure.Setting;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class MassTransit
{
    public static void AddMasstransit(this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMq = configuration.GetSection(RabbitMqOption.SettingName)
            .Get<RabbitMqOption>()!;
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