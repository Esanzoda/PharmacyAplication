using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Order.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Deliver.Commands;

public record ShippedOrderCommand(
    long OrderId,
    long DeliverId) : IRequest<OrderResponseForDeliver>;

public class ReserveOrderToDeliverCommandHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<ShippedOrderCommand, OrderResponseForDeliver>
{
    public async Task<OrderResponseForDeliver> Handle(ShippedOrderCommand request, CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FirstOrDefaultAsync(x => x.Id == request.DeliverId, cancellationToken);

        var order = await dbContext.Orders
            .FirstOrDefaultAsync(x => x.Id == request.OrderId &&
                                      x.Deliver == null, cancellationToken);
        if (order == null)
        {
            throw new ResourceNotFoundException("Order  already has deliver ");
        }

        order.Deliver = deliver;
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<OrderResponseForDeliver>(order);
    }
}