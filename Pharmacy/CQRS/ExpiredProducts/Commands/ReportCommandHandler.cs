using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Data;
using Pharmacy.Jobs;
using Pharmacy.Messages.Events;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.ExpiredProducts.Commands;

public record ReportCommand : IRequest;

public class ReportCommandHandler(
    AppDbContext dbContext,
    ILogger<CheckExpiredProductsJob> logger,
    IPublishEndpoint publishEndpoint) : IRequestHandler<ReportCommand>
{
    public async Task Handle(ReportCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting check to repo");
        var yesterday = DateTime.UtcNow; //.AddDays(-1);
        decimal totalAmount = 0;
        var completedOrders = await dbContext.Orders
            .Where(x => x.OrderStatus == OrderStatus.Completed &&
                        x.CreatedAt==yesterday)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        foreach (var order in completedOrders)
        {
            totalAmount += order.TotalAmount;
        }

        await publishEndpoint.Publish(new OrderCompletedEventReportToCeo()
        {
            To = "orashesanov05@gmail.com",
            Count = completedOrders.Count,
            Day = DateTime.UtcNow,
            TotalAmount = totalAmount
        }, cancellationToken);
        logger.LogInformation("OrderCompletedEventReportToCeo published");
        var cancelledOrders = await dbContext.Orders
            .Where(x => x.OrderStatus == OrderStatus.Cancelled
                        && x.CreatedAt == yesterday)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        await publishEndpoint.Publish(new OrderCancelledEventToCeo()
        {
            DateTime = yesterday,
            Count = cancelledOrders.Count
        }, cancellationToken);
        var sheepadOrders = await dbContext.Orders
            .Where(x => x.OrderStatus == OrderStatus.Shipped && 
                        x.CreatedAt == yesterday)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        await publishEndpoint.Publish(new OrderShippedEventToCeo
        {
            Count = sheepadOrders.Count,
            DateTime = yesterday
        }, cancellationToken);
    }
}