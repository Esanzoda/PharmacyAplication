using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.CQRS.Category.Mapper;
using ProductService.Application.DTOs.Request;
using ProductService.Application.DTOs.Response;
using ProductService.Application.Exceptions;

namespace ProductService.Application.CQRS.Category.Commands;

public record CreateCategoryCommand(
    CreateCategoryRequest Request) : IRequest<CategoryResponse>;

public class CreateCategoryCommandHandler(
    IProductDbContext dbContext) : IRequestHandler<CreateCategoryCommand, CategoryResponse>
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