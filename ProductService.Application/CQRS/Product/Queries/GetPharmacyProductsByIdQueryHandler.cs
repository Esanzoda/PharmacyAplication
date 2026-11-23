using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.CQRS.Product.Mappers;
using ProductService.Application.DTOs.Response;
using ProductService.Application.Exceptions;

namespace ProductService.Application.CQRS.Product.Queries;

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
            ? throw new ResourceNotFoundException("ProductService.API not found")
            : ProductMappers.ToProductForPharmacyResponse(product);
    }
}