using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.CQRS.Product.Mappers;
using ProductService.Application.DTOs.Request;
using ProductService.Application.DTOs.Response;

namespace ProductService.Application.CQRS.Product.Queries.Customer;

public record GetProductsBySalePriceQuery(
    decimal Price,
    PaginationRequest PaginationRequest) : IRequest<List<ProductForCustomerResponse>>;

public class GetProductsByOrderPriseQueryHandler(
    IProductDbContext dbContext) : IRequestHandler<GetProductsBySalePriceQuery, List<ProductForCustomerResponse>>
{
    public async Task<List<ProductForCustomerResponse>> Handle(
        GetProductsBySalePriceQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductBatches)
            .Where(x => x.SalePrice == request.Price)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForCustomerResponse(products);
    }
}