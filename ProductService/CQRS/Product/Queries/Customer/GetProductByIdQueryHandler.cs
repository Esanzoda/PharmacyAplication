using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using ProductService.CQRS.Product.Mappers;
using ProductService.Exception;
using ProductService.Interfaces;

namespace ProductService.CQRS.Product.Queries.Customer;

public record GetProductByIdQuery(
    long Id) : IRequest<ProductForCustomerResponse>;

public class GetProductByIdQueryHandler(
    IProductDbContext dbContext) : IRequestHandler<GetProductByIdQuery, ProductForCustomerResponse>
{
    public async Task<ProductForCustomerResponse> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductBatches)
            .FirstOrDefaultAsync(x => x.Id == request.Id,
                cancellationToken);
        return product is null
            ? throw new ResourceNotFoundException("ProductService not found")
            : ProductMappers.ToProductForCustomerResponse(product);
    }
}