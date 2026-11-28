using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Api.Application.CQRS.Product.Mappers;
using ProductService.Api.Application.DTOs.Request;
using ProductService.Api.Application.DTOs.Response;

namespace ProductService.Api.Application.CQRS.Product.Queries;

public record GetPharmacyProductsBySalePriceQuery(
    long PharmacyId,
    decimal Price,
    PaginationRequest PaginationRequest) : IRequest<List<ProductForPharmacyResponse>>;

public class GetPharmacyProductsBySalePriseQueryHandler(
    IProductDbContext dbContext)
    : IRequestHandler<GetPharmacyProductsBySalePriceQuery, List<ProductForPharmacyResponse>>
{
    public async Task<List<ProductForPharmacyResponse>> Handle(
        GetPharmacyProductsBySalePriceQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.SalePrice == request.Price)
            .Include(x => x.ProductBatches)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForPharmacyResponse(products);
    }
}