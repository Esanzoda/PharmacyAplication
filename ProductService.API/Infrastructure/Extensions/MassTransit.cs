using MassTransit;
using ProductService.Api.Infrastructure.Setting;

namespace ProductService.Api.Infrastructure.Extensions;

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