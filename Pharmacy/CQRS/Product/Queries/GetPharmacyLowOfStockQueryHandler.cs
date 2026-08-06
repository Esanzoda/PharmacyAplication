using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries;

public record GetLowOfStockQuery(
    long PharmacyId,
    int MinQuantity,
    int Page,
    int PageSize) : IRequest<List<ProductWithBatchResponse>>;

public class GetPharmacyLowOfStockQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetLowOfStockQuery, List<ProductWithBatchResponse>>
{
    public async Task<List<ProductWithBatchResponse>> Handle(GetLowOfStockQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Stock <= request.MinQuantity)
            .Include(x => x.ProductBatches)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (!products.Any())
            throw new RecourseNotFoundException("Product not found");

        return mapper.Map<List<ProductWithBatchResponse>>(products);
    }
}