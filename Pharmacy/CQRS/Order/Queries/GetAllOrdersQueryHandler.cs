using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Order.Queries;

public record GetAllOrdersQuery(
    long CustomerId,
    int PageNumber,
    int PageSize) : IRequest<List<OrderResponse>>;

public class GetAllOrdersQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetAllOrdersQuery, List<OrderResponse>>
{
    public async Task<List<OrderResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Where(x=>x.CustomerId==request.CustomerId)
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return mapper.Map<List<OrderResponse>>(orders);
    }
}