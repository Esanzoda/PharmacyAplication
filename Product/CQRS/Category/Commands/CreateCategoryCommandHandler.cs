using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Category.DTOs.Request;
using Pharmacy.Domain.Models.Category.DTOs.Response;
using Pharmacy.Exception;
using Product.CQRS.Category.Mapper;
using Product.Interfaces;

namespace Product.CQRS.Category.Commands;

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