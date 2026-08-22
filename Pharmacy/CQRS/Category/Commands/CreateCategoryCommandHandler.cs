using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Category.Mapper;
using Pharmacy.CQRS.Category.Models.DTOs.Request;
using Pharmacy.CQRS.Category.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Category.Commands;

public record CreateCategoryCommand(
    CreateCategoryRequest Request) : IRequest<CategoryResponse>;

public class CreateCategoryCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var exist = await dbContext.Categories
            .AnyAsync(x => x.Name.ToLower() == request.Request.Name.ToLower(),
                cancellationToken);

        if (exist)
        {
            throw new ResourceIsAlreadyExistException("Category already exists");
        }

        var newcategory = CategoryMappers.ToCategory(request.Request);

        await dbContext.Categories
            .AddAsync(newcategory, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return CategoryMappers.ToCategoryResponse(newcategory);
    }
}