using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Category.Queries;

public record GetCategoryByIdWithProductsQuery(
    int CategoryId,
    int PageNumber,
    int PageSize) : IRequest<List<ProductResponse>>;

public class GetCategoryByIdWithProductsHandler(
    IMapper mapper,
    IApplicationDbContext dbContext)
    :
        IRequestHandler<GetCategoryByIdWithProductsQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetCategoryByIdWithProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products =
            await dbContext.Categories
                .Include(x=>x.Products)
                .Where(x => x.Id == request.CategoryId)
                .OrderBy(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .AsNoTracking() 
                .ToListAsync(cancellationToken);
        if (products.Count == 0)
        {
            throw new RecourseNotFoundException("In this category doesnt exist products ");
        }

        return mapper.Map<List<ProductResponse>>(products);
    }
}