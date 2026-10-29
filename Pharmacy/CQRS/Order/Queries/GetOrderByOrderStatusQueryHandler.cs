using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Order.Mapper;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Order.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Order.Queries;

public record GetOrderByOrderStatusQuery(
    long CustomerId,
    OrderStatus OrderStatus,
    PaginationRequest PaginationRequest) : IRequest<List<OrderResponseForCustomer>>;

public class GetOrderByOrderStatusQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetOrderByOrderStatusQuery, List<OrderResponseForCustomer>>
{
    public async Task<List<OrderResponseForCustomer>> Handle(
        GetOrderByOrderStatusQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Where(x => x.CustomerEntityId == request.CustomerId &&
                        x.OrderStatus == request.OrderStatus)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return OrderMappers.ToListOrderResponseForCustomers(orders);
    }
}