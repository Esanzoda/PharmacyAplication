using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response.Customer;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries.Customer;

public record GetProductByIdQuery(
    long Id) : IRequest<ProductForCustomerResponse>;

public class GetProductByIdQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetProductByIdQuery, ProductForCustomerResponse>
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
        if (product == null)
        {
            throw new ResourceNotFoundException("Product not found");
        }

        return mapper.Map<ProductForCustomerResponse>(product);
    }
}