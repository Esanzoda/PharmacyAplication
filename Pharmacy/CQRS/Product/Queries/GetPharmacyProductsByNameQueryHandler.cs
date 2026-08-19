using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries;

public record GetPharmacyProductsByNameQuery(
    long PharmacyId,
    string Name,
    int Page,
    int PageSize) : IRequest<List<ProductForPharmacyResponse>>;

public class GetPharmacyProductsByNameQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetPharmacyProductsByNameQuery, List<ProductForPharmacyResponse>>
{
    public async Task<List<ProductForPharmacyResponse>> Handle(
        GetPharmacyProductsByNameQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Name.Contains(request.Name))
            .Include(x => x.ProductBatches)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<ProductForPharmacyResponse>>(product);
    }
}