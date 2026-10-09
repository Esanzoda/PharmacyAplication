using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Pharmacy.Exception;
using Product.CQRS.Product.Mappers;
using Product.Interfaces;

namespace Product.CQRS.Product.Queries;

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