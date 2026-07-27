using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Queries;

public record GetProductByBarcodeQuery(
    long PharmacyId,
    string Barcode) : IRequest<ProductResponse>;

public class GetPharmacyProductByBarcodeQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetProductByBarcodeQuery, ProductResponse>
{
    public async Task<ProductResponse> Handle(GetProductByBarcodeQuery request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Barcode == request.Barcode,
                cancellationToken);
        if (product == null)
            throw new RecourseNotFoundException("Product with this barcode not found");

        return mapper.Map<ProductResponse>(product);
    }
}