using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Api.Application.CQRS.Product.Mappers;
using ProductService.Api.Application.DTOs.Request;
using ProductService.Api.Application.DTOs.Response;
using ProductService.Api.Application.Exception;

namespace ProductService.Api.Application.CQRS.Product.Queries.Customer;

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