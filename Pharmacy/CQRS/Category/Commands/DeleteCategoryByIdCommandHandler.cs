using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Category.Commands;

public record DeleteCategoryCommand(
    long Id) : IRequest<bool>;

public class DeleteCategoryByIdHandler(
    IApplicationDbContext dbContext,
    IDistributedCache cache) : IRequestHandler<DeleteCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .FindAsync(request.Id, cancellationToken);

        if (category is null)
        {
            throw new RecourseNotFoundException("Category not found  ");
        }

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync(cancellationToken);
        var key = $"CategoryById-{request.Id}";
        await cache.RemoveAsync(key, cancellationToken);
        return true;
    }
}