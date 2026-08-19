using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries;

public record GetPharmacyLowOfStockQuery(
    long PharmacyId,
    long Id) : IRequest<ProductForPharmacyResponse>;

public class GetPharmacyProductsByIdQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetPharmacyLowOfStockQuery, ProductForPharmacyResponse>
{
    public async Task<ProductForPharmacyResponse> Handle(
        GetPharmacyLowOfStockQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductBatches)
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Id,
                cancellationToken);

        return mapper.Map<ProductForPharmacyResponse>(product);
    }
}