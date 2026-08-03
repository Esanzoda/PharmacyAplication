using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Category.Models.DTOs.Response;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Category.Queries;

public record GetCategoriesByStatusQuery(
    CategoryStatus CategoryStatus,
    int PageNumber,
    int PageSize) : IRequest<List<CategoryResponse>>;

public class GetActiveCategoriesHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<GetCategoriesByStatusQuery, List<CategoryResponse>>
{
    public async Task<List<CategoryResponse>> Handle(GetCategoriesByStatusQuery request,
        CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .Where(x => x.CategoryStatus == request.CategoryStatus)
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return mapper.Map<List<CategoryResponse>>(category);
    }
}