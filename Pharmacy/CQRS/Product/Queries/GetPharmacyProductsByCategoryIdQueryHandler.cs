

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Queries;

public record GetPharmacyProductsByCategoryIdQuery(long PharmacyId,
    long CategoryId,
    int Page,
    int PageSize) : IRequest<List<ProductResponse>>;

public class GetPharmacyProductsByCategoryIdQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetPharmacyProductsByCategoryIdQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetPharmacyProductsByCategoryIdQuery request,
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
            .Where(x =>x.PharmacyId==request.PharmacyId && 
                       x.CategoryId == request.CategoryId)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (!product.Any())
            throw new RecourseNotFoundException("We dont have product  with categoryId ");
        return mapper.Map<List<ProductResponse>>(product);
    }
}