using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Order.Mapper;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;

namespace PharmacyService.Api.Application.CQRS.Order.Queries;

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