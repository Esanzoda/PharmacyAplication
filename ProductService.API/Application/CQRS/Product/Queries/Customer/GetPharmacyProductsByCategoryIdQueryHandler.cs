using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Api.Application.CQRS.Product.Mappers;
using ProductService.Api.Application.DTOs.Request;
using ProductService.Api.Application.DTOs.Response;

namespace ProductService.Api.Application.CQRS.Product.Queries.Customer;

public record GetPharmacyProductsByCategoryIdQuery(
    long PharmacyId,
    long CategoryId,
    PaginationRequest PaginationRequest) : IRequest<List<ProductForCustomerResponse>>;

public class GetPharmacyProductsByCategoryIdQueryHandler(
    IProductDbContext dbContext)
    : IRequestHandler<GetPharmacyProductsByCategoryIdQuery, List<ProductForCustomerResponse>>
{
    public async Task<List<ProductForCustomerResponse>> Handle(
        GetPharmacyProductsByCategoryIdQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.CategoryEntityId == request.CategoryId)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForCustomerResponse(products);
    }
}