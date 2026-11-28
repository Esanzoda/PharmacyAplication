using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Api.Application.CQRS.Product.Mappers;
using ProductService.Api.Application.DTOs.Response;
using ProductService.Api.Application.Exception;

namespace ProductService.Api.Application.CQRS.Product.Queries.Customer;

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
            ? throw new ResourceNotFoundException("ProductService.API not found")
            : ProductMappers.ToProductForCustomerResponse(product);
    }
}