using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries;

public record GetProductsByPurchasePriceQuery(
    long PharmacyId,
    decimal Price,
    int Page,
    int PageSize) : IRequest<List<ProductWithBatchResponse>>;

public class GetProductsByPurchasePriceQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper)
    : IRequestHandler<GetProductsByPurchasePriceQuery, List<ProductWithBatchResponse>>
{
    public async Task<List<ProductWithBatchResponse>> Handle(GetProductsByPurchasePriceQuery request,
        CancellationToken cancellationToken)
    {
        var productBatch = await dbContext.ProductBatches
            .Where(x => x.PurchasePrice == request.Price &&
                        x.Product.PharmacyId == request.PharmacyId)
            .ToListAsync(cancellationToken);

        var productIds = productBatch
            .Select(x => x.ProductId)
            .ToList();
        var products = await dbContext.Products
            .Where(x => productIds.Contains(x.Id))
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (!products.Any())
            throw new RecourseNotFoundException("Product with this purchase price  not found");


        return mapper.Map<List<ProductWithBatchResponse>>(products);
    }
}