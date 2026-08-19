using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries;

public record GetLowOfStockQuery(
    long PharmacyId,
    int MinQuantity,
    int Page,
    int PageSize) : IRequest<List<ProductForPharmacyResponse>>;

public class GetPharmacyLowOfStockQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetLowOfStockQuery, List<ProductForPharmacyResponse>>
{
    public async Task<List<ProductForPharmacyResponse>> Handle(
        GetLowOfStockQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Stock <= request.MinQuantity)
            .Include(x => x.ProductBatches)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<ProductForPharmacyResponse>>(products);
    }
}