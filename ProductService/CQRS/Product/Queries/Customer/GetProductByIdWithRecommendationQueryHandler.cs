using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using ProductService.CQRS.Product.Mappers;
using ProductService.Exception;
using ProductService.Interfaces;

namespace ProductService.CQRS.Product.Queries.Customer;

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

        var productNameSplit = product.Name.ToLower().Split(' ');
        var productName = productNameSplit[0];
        var recommendationProduct = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.PharmacyId == product.PharmacyId &&
                        x.Name.ToLower()
                            .Contains(productName)
            )
            .OrderBy(x => x.SalePrice)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken: cancellationToken);
        return ProductMappers.ToListProductForCustomerResponse(recommendationProduct);
    }
}