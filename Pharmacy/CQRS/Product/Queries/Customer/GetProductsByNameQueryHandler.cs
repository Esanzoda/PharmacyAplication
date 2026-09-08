using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.Mapper;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries.Customer;

public record GetProductsByNameQuery(
    string Name,
    int Page,
    int PageSize) : IRequest<List<ProductForCustomerResponse>>;

public class GetProductsByNameQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetProductsByNameQuery, List<ProductForCustomerResponse>>
{
    public async Task<List<ProductForCustomerResponse>> Handle(
        GetProductsByNameQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductBatches)
            .Where(x => x.Name.ToLower().Contains(request.Name.ToLower()))
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForCustomerResponse(products);
    }
}