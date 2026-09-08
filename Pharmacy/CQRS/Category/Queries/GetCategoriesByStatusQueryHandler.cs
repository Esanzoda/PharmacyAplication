using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Category.Mapper;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Category.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Category.Queries;

public record GetCategoriesByStatusQuery(
    CategoryStatus CategoryStatus,
    int PageNumber,
    int PageSize) : IRequest<List<CategoryResponse>>;

public class GetActiveCategoriesHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetCategoriesByStatusQuery, List<CategoryResponse>>
{
    public async Task<List<CategoryResponse>> Handle(
        GetCategoriesByStatusQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await dbContext.Categories
            .Where(x => x.CategoryStatus == request.CategoryStatus)
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return CategoryMappers.ToCategoriesResponse(categories);
    }
}