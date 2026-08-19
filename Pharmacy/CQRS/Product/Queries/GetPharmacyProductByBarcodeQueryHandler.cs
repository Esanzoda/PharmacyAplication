using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries;

public record GetProductByBarcodeQuery(
    long PharmacyId,
    string Barcode) : IRequest<ProductForPharmacyResponse>;

public class GetPharmacyProductByBarcodeQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<
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

        return mapper.Map<ProductForPharmacyResponse>(product);
    }
}