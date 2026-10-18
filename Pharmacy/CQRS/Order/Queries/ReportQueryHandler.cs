using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Data;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Event.Events;
using Pharmacy.Exception;

namespace Pharmacy.CQRS.Order.Queries;

public record ReportQuery: IRequest;

public class ReportQueryHandler(
    AppDbContext dbContext,
    ILogger<ReportQueryHandler> logger,
    IPublishEndpoint publishEndpoint) : IRequestHandler<ReportQuery>
{
    public async Task Handle(ReportQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting check to repo");
        var today = DateTime.UtcNow.Date;
        var yesterday = today.AddDays(-1);

        var completedOrders = await dbContext.Orders
            .Where(x => x.OrderStatus == OrderStatus.Completed &&
                        x.CreatedAt >= yesterday &&
                        x.CreatedAt < today.AddDays(1))
            .OrderBy(x => x.Id)
            .AsNoTracking()
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
                throw new ResourceNotFoundException("Pharmacy not found");
            }

            var totalAmount = pharmacy.Sum(x => x.TotalAmount);

            await publishEndpoint.Publish(new OrderCompletedEventReportToCeo()
                {
                    To = pharmacyInfo.Email,
                    Count = pharmacy.Count(),
                    Day = today,
                    TotalAmount = totalAmount
                },
                cancellationToken);

            logger.LogInformation("OrderCompletedEventReportToCeo published");
        }
    }
}