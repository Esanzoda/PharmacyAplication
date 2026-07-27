using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Order.Queries;

public record GetOrderByIdQuery(
    long CustomerId,
    long Id) : IRequest<OrderResponse>;

public class GetOrderByIdQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<GetOrderByIdQuery, OrderResponse>
{
    public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Orders
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId &&
                                      x.Id == request.Id,
                cancellationToken);
        if (customer == null)
        {
            throw new RecourseNotFoundException("Order not found");
        }

        return mapper.Map<OrderResponse>(customer);
    }
}