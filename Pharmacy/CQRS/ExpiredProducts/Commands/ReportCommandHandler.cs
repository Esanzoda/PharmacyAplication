using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Data;
using Pharmacy.Event.Events;
using Pharmacy.Jobs;


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
        var today = DateTime.UtcNow.Date.AddDays(1);
        var yesterday = today.AddDays(-2);

        var completedOrders = await dbContext.Orders
            .Where(x => //x.OrderStatus == OrderStatus.Completed &&
                x.CreatedAt >= yesterday &&
                x.CreatedAt < today)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        if (completedOrders.Count == 0)
        {
            return;
        }

        var pharmacyGroup = completedOrders
            .GroupBy(x => x.PharmacyId)
            .ToList();

        var pharmacyIds = pharmacyGroup
            .Select(x => x.Key)
            .ToList();

        var pharmacies = await dbContext.Pharmacies
            .Where(x => pharmacyIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id,
                cancellationToken);

        foreach (var pharmacy in pharmacyGroup)
        {
            if (!pharmacies.TryGetValue(pharmacy.Key, out var pharmacyInfo))
            {
                logger.LogWarning(
                    "Pharmacy {PharmacyId} not found.",
                    pharmacy.Key);
                continue;
            }

            var totalAmount = pharmacy.Sum(x => x.TotalAmount);

            await publishEndpoint.Publish(new OrderCompletedEventReportToCeo()
                {
                    To = pharmacyInfo.Email,
                    Count = pharmacy.Count(),
                    Day = DateTime.UtcNow,
                    TotalAmount = totalAmount
                },
                cancellationToken);

            logger.LogInformation("OrderCompletedEventReportToCeo published");
        }
    }
}