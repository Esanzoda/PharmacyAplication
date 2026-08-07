using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries;

public record GetAllPharmacyProductsQuery(
    long PharmacyId,
    int Page,
    int PageSize) : IRequest<List<ProductWithBatchResponse>>;

public class GetAllPharmacyProductsQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetAllPharmacyProductsQuery, List<ProductWithBatchResponse>>
{
    public async Task<List<ProductWithBatchResponse>> Handle(GetAllPharmacyProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .Include(x => x.ProductBatches)
            .Where(x => x.PharmacyId == request.PharmacyId)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return mapper.Map<List<ProductWithBatchResponse>>(products);
    }
}