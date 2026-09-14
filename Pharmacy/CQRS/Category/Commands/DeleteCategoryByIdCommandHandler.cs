using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Category.Commands;

public record DeleteCategoryCommand(
    long Id) : IRequest<string>;

public class DeleteCategoryByIdHandler(
    IApplicationDbContext dbContext,
    IDistributedCache cache) : IRequestHandler<DeleteCategoryCommand,string >
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
        var key = $"CategoryById-{request.Id}";
        await cache.RemoveAsync(key, cancellationToken);
        

        return Message.Deleted  ;
    }

    
}