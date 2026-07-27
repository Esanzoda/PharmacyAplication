using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Order.Queries;

public record GetOrderByOrderStatusQuery(
    long CustomerId,
    OrderStatus OrderStatus,
    int PageNumber,
    int PageSize)
    : IRequest<List<OrderResponse>>;

public class GetOrderByOrderStatusQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<GetOrderByOrderStatusQuery, List<OrderResponse>>
{
    public async Task<List<OrderResponse>> Handle(GetOrderByOrderStatusQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Where(x => x.CustomerId == request.CustomerId &&
                        x.OrderStatus == request.OrderStatus)
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);


        return mapper.Map<List<OrderResponse>>(orders);
    }
}