using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.Mapper;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries.Customer;

public record GetProductsByCountryQuery(
    CountryEnum Country,
    int Page,
    int PageSize) : IRequest<List<ProductForCustomerResponse>>;

public class GetProductsByCountryQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetProductsByCountryQuery, List<ProductForCustomerResponse>>
{
    public async Task<List<ProductForCustomerResponse>> Handle(
        GetProductsByCountryQuery request,
        CancellationToken cancellationToken)
    {
        var productBatch = await dbContext.ProductBatches
            .AsNoTracking()
            .Where(x => x.Country == request.Country)
            .ToListAsync(cancellationToken);

        var productIds = productBatch
            .Select(x => x.ProductEntityId)
            .ToList();

        var products = await dbContext.Products
            .AsNoTracking()
            .Where(x => productIds.Contains(x.Id))
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForCustomerResponse(products);
    }
}