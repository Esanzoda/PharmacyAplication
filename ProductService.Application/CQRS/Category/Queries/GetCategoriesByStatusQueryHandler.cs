using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.CQRS.Category.Mapper;
using ProductService.Application.DTOs.Request;
using ProductService.Application.DTOs.Response;
using ProductServiceDomain.Enums;

namespace ProductService.Application.CQRS.Category.Queries;

public record GetCategoriesByStatusQuery(
    CategoryStatus CategoryStatus,
    PaginationRequest PaginationRequest) : IRequest<List<CategoryResponse>>;

public class GetActiveCategoriesHandler(
    IProductDbContext dbContext) : IRequestHandler<GetCategoriesByStatusQuery, List<CategoryResponse>>
{
    public async Task<List<CategoryResponse>> Handle(
        GetCategoriesByStatusQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await dbContext.Categories
            .Where(x => x.CategoryStatus == request.CategoryStatus)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return CategoryMappers.ToCategoriesResponse(categories);
    }
}