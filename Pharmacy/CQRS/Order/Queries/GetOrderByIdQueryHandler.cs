using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Order.Mapper;
using Pharmacy.Domain.Models.Order.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Order.Queries;

public record GetOrderByIdQuery(
    long CustomerId,
    long Id) : IRequest<OrderResponseForCustomer>;

public class GetOrderByIdQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetOrderByIdQuery,
    OrderResponseForCustomer>
{
    public async Task<OrderResponseForCustomer> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.CustomerEntityId == request.CustomerId &&
                                      x.Id == request.Id,
                cancellationToken);

        return order == null
            ? throw new ResourceNotFoundException("Order not found")
            : OrderMappers.ToOrderResponseForCustomer(order);
    }
}