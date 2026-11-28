using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Api.Application.CQRS.Product.Mappers;
using ProductService.Api.Application.DTOs.Request;
using ProductService.Api.Application.DTOs.Response;
using ProductService.Api.Application.Exception;

namespace ProductService.Api.Application.CQRS.Product.Queries;

public record GetPharmacyProductsByCategoryIdQuery(
    long PharmacyId,
    long CategoryId,
    PaginationRequest PaginationRequest) : IRequest<List<ProductForPharmacyResponse>>;

public class GetPharmacyProductsByCategoryIdQueryHandler(
    IProductDbContext dbContext)
    : IRequestHandler<GetPharmacyProductsByCategoryIdQuery, List<ProductForPharmacyResponse>>
{
    public async Task<List<ProductForPharmacyResponse>> Handle(
        GetPharmacyProductsByCategoryIdQuery request,
        CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.CategoryId,
                cancellationToken);

        if (!category)
        {
            throw new ResourceNotFoundException("Category with this id  not found");
        }

        var products = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.CategoryEntityId == request.CategoryId)
            .Include(x => x.ProductBatches)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForPharmacyResponse(products);
    }
}