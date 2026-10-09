using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Product.CQRS.Product.Mappers;
using Product.Interfaces;

namespace Product.CQRS.Product.Queries.Customer;

public record GetProductsByCountryQuery(
    CountryEnum Country,
    PaginationRequest PaginationRequest) : IRequest<List<ProductForCustomerResponse>>;

public class GetProductsByCountryQueryHandler(
    IProductDbContext dbContext) : IRequestHandler<GetProductsByCountryQuery, List<ProductForCustomerResponse>>
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
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForCustomerResponse(products);
    }
}