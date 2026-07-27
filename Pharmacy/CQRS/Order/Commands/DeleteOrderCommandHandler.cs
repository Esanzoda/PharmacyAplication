using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Order.Commands;

public record DeleteOrderCommand(
    long CustomerId,
    long OrderId) : IRequest<bool>;

public class DeleteOrderHandler(
    IApplicationDbContext dbContext) : IRequestHandler<DeleteOrderCommand, bool>
{
    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId &&
                                      x.Id == request.OrderId,
                cancellationToken);
        if (order == null)
        {
            throw new RecourseNotFoundException($"Order with id {request.OrderId} not found");
        }

        dbContext.Orders
            .Remove(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}