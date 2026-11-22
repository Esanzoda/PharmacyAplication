using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.CQRS.Product.Mappers;
using ProductService.Application.DTOs.Request;
using ProductService.Application.DTOs.Response;
using ProductServiceDomain.Enums;

namespace ProductService.Application.CQRS.Product.Queries;

public record GetPharmacyProductsByCountryQuery(
    long PharmacyId,
    CountryEnum Country,
    PaginationRequest PaginationRequest) : IRequest<List<ProductForPharmacyResponse>>;

public class GetPharmacyProductsByCountryQueryHandler(
    IProductDbContext dbContext)
    : IRequestHandler<GetPharmacyProductsByCountryQuery, List<ProductForPharmacyResponse>>
{
    public async Task<List<ProductForPharmacyResponse>> Handle(
        GetPharmacyProductsByCountryQuery request,
        CancellationToken cancellationToken)
    {
        var productBatch = await dbContext.ProductBatches
            .Where(x => x.Country == request.Country &&
                        x.PharmacyId == request.PharmacyId)
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

        return ProductMappers.ToListProductForPharmacyResponse(products);
    }
}