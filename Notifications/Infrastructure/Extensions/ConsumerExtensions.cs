using MassTransit;
using Notifications.Consumers;
using Notifications.Setting;

namespace Notifications.Infrastructure.Extensions;

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
                x.AddConsumer<OrderCreatedConsumer>()
                    .Endpoint(e => e.Name = "order-created");
                x.AddConsumer<OrderCancelledConsumer>()
                    .Endpoint(e => e.Name = "order-cancelled");
                x.AddConsumer<OrderCompletedConsumer>()
                    .Endpoint(e => e.Name = "order-completed");
                x.AddConsumer<ReportToCeoOrderCompleted>()
                    .Endpoint(e => e.Name = "report-to-ceo-order-completed");
                x.AddConsumer<CheckExpiryDateProductConsumer>()
                    .Endpoint(e => e.Name = "check-expiry-date-product");
                x.AddConsumer<OrderShippedConsumer>()
                    .Endpoint(e => e.Name = "order-shipped");
                x.AddConsumer<ForgotPasswordConsumer>()
                    .Endpoint(e=>e.Name="password-updated");
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