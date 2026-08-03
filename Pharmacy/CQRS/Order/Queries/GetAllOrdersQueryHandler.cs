using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Order.Models.DTOs.Response;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Order.Queries;

public record GetAllOrdersQuery(
    long CustomerId,
    int PageNumber,
    int PageSize) : IRequest<List<OrderResponseForCustomer>>;

public class GetAllOrdersQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetAllOrdersQuery, List<OrderResponseForCustomer>>
{
    public async Task<List<OrderResponseForCustomer>> Handle(GetAllOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(x => x.OrderItems)
            .Where(x => x.CustomerId == request.CustomerId)
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return mapper.Map<List<OrderResponseForCustomer>>(orders);
    }
}