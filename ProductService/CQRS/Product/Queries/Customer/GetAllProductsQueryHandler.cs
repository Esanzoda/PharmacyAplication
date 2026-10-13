using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using ProductService.CQRS.Product.Mappers;
using ProductService.Interfaces;

namespace ProductService.CQRS.Product.Queries.Customer;

public record GetAllProductsQuery(
    PaginationRequest PaginationRequest) : IRequest<List<ProductForCustomerResponse>>;

public class GetAllProductsQueryHandler(
    IProductDbContext dbContext) : IRequestHandler<GetAllProductsQuery, List<ProductForCustomerResponse>>
{
    public async Task<List<ProductForCustomerResponse>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .Include(x => x.ProductBatches)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForCustomerResponse(products);
    }
}