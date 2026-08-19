using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response.Customer;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries.Customer;

public record GetProductsBySalePriceQuery(
    decimal Price,
    int Page,
    int PageSize) : IRequest<List<ProductForCustomerResponse>>;

public class GetProductsByOrderPriseQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetProductsBySalePriceQuery, List<ProductForCustomerResponse>>
{
    public async Task<List<ProductForCustomerResponse>> Handle(
        GetProductsBySalePriceQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductBatches)
            .Where(x => x.SalePrice == request.Price)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<ProductForCustomerResponse>>(product);
    }
}