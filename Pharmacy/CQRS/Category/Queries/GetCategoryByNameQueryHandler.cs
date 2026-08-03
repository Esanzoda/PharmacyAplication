using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Category.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Category.Queries;

public record GetCategoryByNameQuery(
    string Name) : IRequest<List<CategoryResponse>>;

public class GetCategoryByNameQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<GetCategoryByNameQuery, List<CategoryResponse>>
{
    public async Task<List<CategoryResponse>> Handle(GetCategoryByNameQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await dbContext.Categories
            .Where(x => x.Name.Contains(request.Name))
            .OrderBy(x => x.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (categories.Count == 0)
        {
            throw new RecourseNotFoundException("Category not found");
        }

        return mapper.Map<List<CategoryResponse>>(categories);
    }
}