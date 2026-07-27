using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Deliver.Commands;

public record UpdateOrderStatusCommand(
    long PharmacyId,
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
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.OrderId &&
                                      x.Deliver != null &&
                                      x.Deliver.Id == request.DeliverId, cancellationToken);
        if (order is null)
        {
            throw new RecourseNotFoundException("Order not found");
        }

        if (request.NewOrderStatus is not (OrderStatus.Cancelled or OrderStatus.Shipped))
        {
            throw new BusinessException("You cannot choice other status");
        }

        order.OrderStatus = request.NewOrderStatus;
        await dbContext.SaveChangesAsync(cancellationToken);
        return order.OrderStatus;
    }
}