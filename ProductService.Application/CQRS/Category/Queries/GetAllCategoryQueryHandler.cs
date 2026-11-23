using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.CQRS.Category.Mapper;
using ProductService.Application.DTOs.Request;
using ProductService.Application.DTOs.Response;

namespace ProductService.Application.CQRS.Category.Queries;

public record GetAllCategoriesByPaginationQuery(
    PaginationRequest PaginationRequest) : IRequest<List<CategoryResponse>>;

public class GetAllCategoryQueryHandler(
    IProductDbContext dbContext) : IRequestHandler<GetAllCategoriesByPaginationQuery, List<CategoryResponse>>
{
    public async Task<List<CategoryResponse>> Handle(
        GetAllCategoriesByPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await dbContext.Categories
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return CategoryMappers.ToCategoriesResponse(categories);
    }
}