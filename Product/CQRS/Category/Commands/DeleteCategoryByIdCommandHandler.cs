using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Exception;
using Product.Interfaces;
using StackExchange.Redis;

namespace Product.CQRS.Category.Commands;

public record DeleteCategoryCommand(
    long Id) : IRequest<string>;

public class DeleteCategoryByIdHandler(
    IProductDbContext dbContext,
    IDistributedCache cache,
    ILogger<DeleteCategoryByIdHandler> logger) : IRequestHandler<DeleteCategoryCommand, string>
{
    public async Task<string> Handle(
        DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .FindAsync([request.Id],
                cancellationToken);

        if (category is null)
        {
            throw new ResourceNotFoundException("Category not found");
        }

        var existsProduct = await dbContext.Products
            .AnyAsync(x => x.CategoryEntityId == request.Id,
                cancellationToken);

        if (existsProduct)
        {
            throw new BusinessException("Cannot delete category with products");
        }

        dbContext.Categories.Remove(category);

        await dbContext.SaveChangesAsync(cancellationToken);
        try
        {
            var key = $"CategoryById-{request.Id}";
            await cache.RemoveAsync(key, cancellationToken);
        }
        catch (RedisConnectionException)
        {
            logger.LogWarning("Redis is not connected");
            logger.LogInformation("Cannot remove data in Redis");
        }


        return Message.Deleted;
    }
}