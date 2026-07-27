using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Category.Commands;

public record UpdateCategoryCommand(
    long Id,
    UpdateCategoryRequest Request) : IRequest<UpdateCategoryResponse>;

public class UpdateCategoryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext,
    IDistributedCache cache)
    : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResponse>
{
    public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .FindAsync(request.Id, cancellationToken);
        if (category is null)
        {
            throw new RecourseNotFoundException("Category not found");
        }

        var existCategory = await dbContext.Categories
            .AnyAsync(x => x.Id != request.Id &&
                           x.Name == request.Request.Name,
                cancellationToken);
        if (existCategory)
        {
            throw new RecourseIsAlreadyExistException("Category  with this name already exist");
        }

        mapper.Map(request.Request, category);
        await dbContext.SaveChangesAsync(cancellationToken);

        var key = $"CategoryById-{request.Id}";
        await cache.RemoveAsync(key, cancellationToken);

        return mapper.Map<UpdateCategoryResponse>(category);
    }
}