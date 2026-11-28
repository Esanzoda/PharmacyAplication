using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ProductService.Api.Application.CQRS.Category.Mapper;
using ProductService.Api.Application.DTOs.Response;
using ProductService.Api.Application.Exception;
using ProductService.Api.Domain.Entities;
using StackExchange.Redis;

namespace ProductService.Api.Application.CQRS.Category.Queries;

public record GetCategoryByIdQuery(
    long CategoryId) : IRequest<CategoryResponse>;

public class GetCategoryByIdQueryHandler(
    IDistributedCache cache,
    IProductDbContext dbContext,
    ILogger<GetCategoryByIdQueryHandler> logger) : IRequestHandler<GetCategoryByIdQuery, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var key = $"CategoryById-{request.CategoryId}";
        try
        {
            var cachedCategory = await cache.GetStringAsync(key, cancellationToken);
            if (cachedCategory is not null)
            {
                var entity = JsonConvert.DeserializeObject<CategoryEntity>(cachedCategory);
                if (entity is not null)
                {
                    return CategoryMappers.ToCategoryResponse(entity);
                }
            }
        }
        catch (RedisConnectionException)
        {
            logger.LogWarning("Redis is not connected");
        }

        var category = await dbContext.Categories
            .FindAsync([request.CategoryId],
                cancellationToken);

        if (category is null)
        {
            throw new ResourceNotFoundException("Category not found");
        }

        try
        {
            await cache.SetStringAsync(key,
                JsonConvert.SerializeObject(category), new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                }, cancellationToken);
        }
        catch (RedisConnectionException)
        {
            logger.LogWarning("Redis is not connected");
            logger.LogInformation("Can not stored data in Redis");
        }
        
        return CategoryMappers.ToCategoryResponse(category);
    }
}