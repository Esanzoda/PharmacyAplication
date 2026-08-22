using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Category.Mapper;
using Pharmacy.CQRS.Category.Models.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Category.Queries;

public record GetCategoryByNameQuery(
    string Name) : IRequest<List<CategoryResponse>>;

public class GetCategoryByNameQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetCategoryByNameQuery, List<CategoryResponse>>
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