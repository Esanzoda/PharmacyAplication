using MassTransit;
using ProductService.Infrastructure.Setting;

namespace ProductService.Infrastructure.Extensions;

public static class MassTransit
{
    public static void AddMasstransit(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var rabbitMq = configuration.GetSection(RabbitMqOption.SettingName)
            .Get<RabbitMqOption>()!;
        serviceCollection.AddMassTransit(x =>
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