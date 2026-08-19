using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Event.Events;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Employee.Commands;

public record UpdateOrderStatusCommand(
    long EmployeeId,
    long PharmacyId,
    long OrderId,
    OrderStatus NewOrderStatus) : IRequest<OrderStatus>;

public class UpdateOrderStatusCommandHandler(
    IApplicationDbContext dbContext,
    IPublishEndpoint publishEndpoint,
    ILogger<UpdateOrderStatusCommandHandler> logger) : IRequestHandler<UpdateOrderStatusCommand, OrderStatus>
{
    public async Task<OrderStatus> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(x => x.CustomerEntity)
            .FirstOrDefaultAsync(x => x.Id == request.OrderId &&
                                      x.PharmacyId == request.PharmacyId,
                cancellationToken);
        if (order == null)
        {
            throw new ResourceNotFoundException("Order not found");
        }

        order.OrderStatus = request.NewOrderStatus;
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Employee {EmployeeId} changed order {OrderId} status from {OldStatus} to {NewStatus}",
            request.EmployeeId,
            request.OrderId,
            order.OrderStatus,
            request.NewOrderStatus);

        await publishEndpoint.Publish(new OrderCompletedEvent
        {
            Email = order.CustomerEntity.Email,
            OrderId = order.Id,
            CustomerId = order.CustomerEntityId,
            TotalAmount = order.TotalAmount,
            CompletedAt = order.UpdateAt,
        }, cancellationToken);

        return order.OrderStatus;
    }
}