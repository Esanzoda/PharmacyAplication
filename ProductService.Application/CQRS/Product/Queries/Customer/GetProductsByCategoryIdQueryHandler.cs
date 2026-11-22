using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.CQRS.Product.Mappers;
using ProductService.Application.DTOs.Request;
using ProductService.Application.DTOs.Response;
using ProductService.Application.Exceptions;

namespace ProductService.Application.CQRS.Product.Queries.Customer;

public record GetProductsByCategoryIdQuery(
    long CategoryId,
    PaginationRequest PaginationRequest) : IRequest<List<ProductForCustomerResponse>>;

public class GetProductsByCategoryIdQueryHandler(
    IProductDbContext dbContext) : IRequestHandler<GetProductsByCategoryIdQuery, List<ProductForCustomerResponse>>
{
    public async Task<List<ProductForCustomerResponse>> Handle(
        GetProductsByCategoryIdQuery request,
        CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .AnyAsync(x => x.Id == request.CategoryId,
                cancellationToken);
        
        if (!category)
        {
            throw new ResourceNotFoundException("Category with this id  not found");
        }

        var products = await dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductBatches)
            .Where(x => x.CategoryEntityId == request.CategoryId)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForCustomerResponse(products);
    }
}