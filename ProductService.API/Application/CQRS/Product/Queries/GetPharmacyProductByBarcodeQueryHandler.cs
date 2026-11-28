using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Api.Application.CQRS.Product.Mappers;
using ProductService.Api.Application.DTOs.Response;
using ProductService.Api.Application.Exception;

namespace ProductService.Api.Application.CQRS.Product.Queries;

public record GetProductByBarcodeQuery(
    long PharmacyId,
    string Barcode) : IRequest<ProductForPharmacyResponse>;

public class GetPharmacyProductByBarcodeQueryHandler(
    IProductDbContext dbContext) : IRequestHandler<
    GetProductByBarcodeQuery,
    ProductForPharmacyResponse>
{
    public async Task<ProductForPharmacyResponse> Handle(
        GetProductByBarcodeQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductBatches)
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Barcode == request.Barcode,
                cancellationToken);

        return product == null
            ? throw new ResourceNotFoundException("Product not found")
            : ProductMappers.ToProductForPharmacyResponse(product);
    }
}