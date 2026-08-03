using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Deliver.Commands;

public record UpdateOrderStatusCommand(
    long DeliverId,
    long OrderId,
    OrderStatus NewOrderStatus)
    : IRequest<OrderStatus>;

public class UpdateOrderStatusCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<UpdateOrderStatusCommand, OrderStatus>
{
    public async Task<OrderStatus> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .FirstOrDefaultAsync(x => x.Id == request.OrderId &&
                                      x.Deliver != null && x.Deliver.Id == request.DeliverId,
                cancellationToken);
        if (order is null)
        {
            throw new RecourseNotFoundException("Order not found");
        }

        if (order.OrderStatus is OrderStatus.Cancelled or OrderStatus.Completed)
        {
            throw new BusinessException($"Deliver cannot update a {order.OrderStatus} order");
        }

        if (request.NewOrderStatus is not (OrderStatus.Shipped or OrderStatus.Completed))
        {
            throw new BusinessException("Deliver can only update the status to Shipped or Completed");
        }

        order.OrderStatus = request.NewOrderStatus;
        await dbContext.SaveChangesAsync(cancellationToken);
        return order.OrderStatus;
    }
}