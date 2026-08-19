using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Category.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Category.Queries;

public record GetCategoryByNameQuery(
    string Name) : IRequest<CategoryResponse>;

public class GetCategoryByNameQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<GetCategoryByNameQuery, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(
        GetCategoryByNameQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await dbContext.Categories
            .Where(x => x.Name.ToLower().Contains(request.Name.ToLower()))
            .OrderBy(x => x.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return categories.Count == 0
            ? throw new ResourceNotFoundException("Category not found")
            : mapper.Map<CategoryResponse>(categories);
    }
}