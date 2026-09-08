using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.CQRS.Category.Mapper;
using Pharmacy.Domain.Models.Category.DTOs.Request;
using Pharmacy.Domain.Models.Category.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Category.Commands;

public record UpdateCategoryCommand(
    long Id,
    UpdateCategoryRequest Request) : IRequest<CategoryResponse>;

public class UpdateCategoryCommandHandler(
    IApplicationDbContext dbContext,
    IDistributedCache cache) : IRequestHandler<UpdateCategoryCommand, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(
        UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .FindAsync([request.Id],
                cancellationToken);

        if (category is null)
        {
            throw new ResourceNotFoundException("Category not found");
        }

        var existCategory = await dbContext.Categories
            .AnyAsync(x => x.Id != request.Id &&
                           x.Name == request.Request.Name,
                cancellationToken);

        if (existCategory)
        {
            throw new ResourceIsAlreadyExistException("Category  with this name already exist");
        }

        CategoryMappers.ToCategory(category, request.Request);
        await dbContext.SaveChangesAsync(cancellationToken);

        var key = $"CategoryById-{request.Id}";
        await cache.RemoveAsync(key, cancellationToken);

        return CategoryMappers.ToCategoryResponse(category);
    }
}