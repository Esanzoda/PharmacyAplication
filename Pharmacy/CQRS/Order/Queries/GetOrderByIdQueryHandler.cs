using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Order.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Order.Queries;

public record GetOrderByIdQuery(
    long CustomerId,
    long Id) : IRequest<OrderResponseForCustomer>;

public class GetOrderByIdQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<GetOrderByIdQuery, OrderResponseForCustomer>
{
    public async Task<OrderResponseForCustomer> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId &&
                                      x.Id == request.Id,
                cancellationToken);
        if (order == null)
        {
            throw new RecourseNotFoundException("Order not found");
        }

        return mapper.Map<OrderResponseForCustomer>(order);
    }
}