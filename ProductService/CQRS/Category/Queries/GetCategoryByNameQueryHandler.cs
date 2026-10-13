using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Category.DTOs.Response;
using ProductService.CQRS.Category.Mapper;
using ProductService.Interfaces;

namespace ProductService.CQRS.Category.Queries;

public record GetCategoryByNameQuery(
    string Name) : IRequest<List<CategoryResponse>>;

public class GetCategoryByNameQueryHandler(
    IProductDbContext dbContext) : IRequestHandler<GetCategoryByNameQuery, List<CategoryResponse>>
{
    public async Task<List<CategoryResponse>> Handle(
        GetCategoryByNameQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await dbContext.Categories
            .Where(x => x.Name.ToLower().Contains(request.Name.ToLower()))
            .OrderBy(x => x.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return CategoryMappers.ToCategoriesResponse(categories);
    }
}