using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries;

public record GetPharmacyProductsByCategoryIdQuery(
    long PharmacyId,
    long CategoryId,
    int Page,
    int PageSize) : IRequest<List<ProductWithBatchResponse>>;

public class GetPharmacyProductsByCategoryIdQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetPharmacyProductsByCategoryIdQuery, List<ProductWithBatchResponse>>
{
    public async Task<List<ProductWithBatchResponse>> Handle(GetPharmacyProductsByCategoryIdQuery request,
        CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.CategoryId, cancellationToken);
        if (!category)
        {
            throw new RecourseNotFoundException("Category with this id  not found");
        }

        var product = await dbContext.Products
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.CategoryId == request.CategoryId)
            .Include(x => x.ProductBatches)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (!product.Any())
            throw new RecourseNotFoundException("We dont have product  with categoryId ");
        return mapper.Map<List<ProductWithBatchResponse>>(product);
    }
}