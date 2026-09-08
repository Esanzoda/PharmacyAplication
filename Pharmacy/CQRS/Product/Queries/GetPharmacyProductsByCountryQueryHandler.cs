using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.Mapper;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries;

public record GetPharmacyProductsByCountryQuery(
    long PharmacyId,
    CountryEnum Country,
    int Page,
    int PageSize) : IRequest<List<ProductForPharmacyResponse>>;

public class GetPharmacyProductsByCountryQueryHandler(
    IApplicationDbContext dbContext)
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
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForPharmacyResponse(products);
    }
}