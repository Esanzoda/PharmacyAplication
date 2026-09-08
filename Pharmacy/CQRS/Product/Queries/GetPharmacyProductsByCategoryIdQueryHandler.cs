using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.Mapper;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries;

public record GetPharmacyProductsByCategoryIdQuery(
    long PharmacyId,
    long CategoryId,
    int Page,
    int PageSize) : IRequest<List<ProductForPharmacyResponse>>;

public class GetPharmacyProductsByCategoryIdQueryHandler(
    IApplicationDbContext dbContext)
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
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForPharmacyResponse(products);
    }
}