using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.CQRS.Product.Mappers;
using ProductService.Application.DTOs.Request;
using ProductService.Application.DTOs.Response;

namespace ProductService.Application.CQRS.Product.Queries;

public record GetProductsByPurchasePriceQuery(
    long PharmacyId,
    decimal Price,
    PaginationRequest PaginationRequest) : IRequest<List<ProductForPharmacyResponse>>;

public class GetProductsByPurchasePriceQueryHandler(
    IProductDbContext dbContext)
    : IRequestHandler<GetProductsByPurchasePriceQuery, List<ProductForPharmacyResponse>>
{
    public async Task<List<ProductForPharmacyResponse>> Handle(
        GetProductsByPurchasePriceQuery request,
        CancellationToken cancellationToken)
    {
        var productBatch = await dbContext.ProductBatches
            .AsNoTracking()
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.PurchasePrice == request.Price)
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