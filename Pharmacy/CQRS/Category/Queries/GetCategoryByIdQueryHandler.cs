using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmacy.CQRS.Category.Mapper;
using Pharmacy.CQRS.Category.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Category.Queries;

public record GetCategoryByIdQuery(
    long CategoryId) : IRequest<CategoryResponse>;

public class GetCategoryByIdHandler(
    IDistributedCache cache,
    IApplicationDbContext dbContext) : IRequestHandler<GetCategoryByIdQuery, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var key = $"CategoryById-{request.CategoryId}";

        var cachedCategory = await cache.GetStringAsync(key, cancellationToken);
        if (cachedCategory is not null)
        {
            var entity = JsonConvert.DeserializeObject<Models.CategoryEntity>(cachedCategory);
            if (entity is not null)
            {
                return CategoryMappers.ToCategoryResponse(entity);
            }
        }

        var category = await dbContext.Categories
            .FindAsync([request.CategoryId],
                cancellationToken);

        if (category is null)
        {
            throw new ResourceNotFoundException("Category not found");
        }

        await cache.SetStringAsync(key,
            JsonConvert.SerializeObject(category), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            }, cancellationToken);

        return CategoryMappers.ToCategoryResponse(category);
    }
}