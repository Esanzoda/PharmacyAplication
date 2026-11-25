using MassTransit;
using Notification.Api_.Consumers;
using Notification.Api_.Infrastructure.Setting;

namespace Notification.Api_.Extensions;

public static class ConsumerExtensions
{
    public static void AddConsumers(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var rabbitmq = configuration.GetSection(RabbitMqOption.SettingName)
            .Get<RabbitMqOption>();
        if (rabbitmq != null)
        {
            serviceCollection.AddMassTransit(x =>
            {
                x.AddConsumer<OrderCreatedConsumer>();
                x.AddConsumer<OrderCancelledConsumer>();
                x.AddConsumer<OrderCompletedConsumer>();
                x.AddConsumer<ReportToCeoOrderCompleted>();
                x.AddConsumer<CheckExpiryDateProductConsumer>();
                x.AddConsumer<OrderShippedConsumer>();
                x.AddConsumer<ForgotPasswordConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitmq.Host, rabbitmq.VirtualHost, hostConfigure =>
                    {
                        hostConfigure.Username(rabbitmq.UserName);
                        hostConfigure.Password(rabbitmq.Password);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}