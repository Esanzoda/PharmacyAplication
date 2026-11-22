using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.CQRS.Product.Mappers;
using ProductService.Application.DTOs.Request;
using ProductService.Application.DTOs.Response;
using ProductService.Application.Exceptions;

namespace ProductService.Application.CQRS.Product.Queries.Customer;

public record GetProductByIdWithRecommendationQuery(
    long ProductId,
    PaginationRequest PaginationRequest) : IRequest<List<ProductForCustomerResponse>>;

public class GetProductByIdWithRecommendationQueryHandler(IProductDbContext dbContext)
    : IRequestHandler<GetProductByIdWithRecommendationQuery, List<ProductForCustomerResponse>>
{
    public async Task<List<ProductForCustomerResponse>> Handle(GetProductByIdWithRecommendationQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ProductId,
                cancellationToken: cancellationToken);

        if (product == null)
        {
            throw new ResourceNotFoundException("Product not found");
        }

        var recommendationProduct = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.PharmacyId == product.PharmacyId &&
                        x.CategoryEntityId == product.CategoryEntityId
            )
            .OrderBy(x => x.SalePrice)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken: cancellationToken);

        return ProductMappers.ToListProductForCustomerResponse(recommendationProduct);
    }
}