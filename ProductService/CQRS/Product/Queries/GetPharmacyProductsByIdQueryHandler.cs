using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Product.DTos.Response;
using ProductService.CQRS.Product.Mappers;
using ProductService.Exception;
using ProductService.Interfaces;

namespace ProductService.CQRS.Product.Queries;

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
            ? throw new ResourceNotFoundException("ProductService not found")
            : ProductMappers.ToProductForPharmacyResponse(product);
    }
}