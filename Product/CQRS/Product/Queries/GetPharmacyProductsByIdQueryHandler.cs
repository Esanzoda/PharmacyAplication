using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Pharmacy.Exception;
using Product.CQRS.Product.Mappers;
using Product.Interfaces;

namespace Product.CQRS.Product.Queries;

public record GetPharmacyProductsByIdQuery(
    long PharmacyId,
    long Id) : IRequest<ProductForPharmacyResponse>;

public class GetPharmacyProductsByIdQueryHandler(
    IProductDbContext dbContext) : IRequestHandler<GetPharmacyProductsByIdQuery, ProductForPharmacyResponse>
{
    public async Task<ProductForPharmacyResponse> Handle(
        GetPharmacyProductsByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductBatches)
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Id,
                cancellationToken);
        return product == null
            ? throw new ResourceNotFoundException("Product not found")
            : ProductMappers.ToProductForPharmacyResponse(product);
    }
}