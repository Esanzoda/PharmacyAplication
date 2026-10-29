using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Order.Mapper;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Order.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Order.Queries;

public record GetAllOrdersQuery(
    long CustomerId,
    PaginationRequest PaginationRequest) : IRequest<List<OrderResponseForCustomer>>;

public class GetAllOrdersQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetAllOrdersQuery, List<OrderResponseForCustomer>>
{
    public async Task<List<OrderResponseForCustomer>> Handle(
        GetAllOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(x => x.OrderItems)
            .Where(x => x.CustomerEntityId == request.CustomerId)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return OrderMappers.ToListOrderResponseForCustomers(orders);
    }
}