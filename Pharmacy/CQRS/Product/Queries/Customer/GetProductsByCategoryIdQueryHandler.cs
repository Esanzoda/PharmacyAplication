using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response.Customer;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries.Customer;

public record GetProductsByCategoryIdQuery(
    long CategoryId,
    int Page,
    int PageSize) : IRequest<List<ProductForCustomerResponse>>;

public class GetProductsByCategoryIdQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetProductsByCategoryIdQuery, List<ProductForCustomerResponse>>
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

        var product = await dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductBatches)
            .Where(x => x.CategoryEntityId == request.CategoryId)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<ProductForCustomerResponse>>(product);
    }
}