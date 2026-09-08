using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.Mapper;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries.Customer;

public record GetProductByIdQuery(
    long Id) : IRequest<ProductForCustomerResponse>;

public class GetProductByIdQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetProductByIdQuery, ProductForCustomerResponse>
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
            ? throw new ResourceNotFoundException("Product not found")
            : ProductMappers.ToProductForCustomerResponse(product);
    }
}