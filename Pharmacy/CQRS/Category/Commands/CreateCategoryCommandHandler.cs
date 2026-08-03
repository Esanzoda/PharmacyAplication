using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Category.Models.DTOs.Request;
using Pharmacy.CQRS.Category.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Category.Commands;

public record CreateCategoryCommand(
    CreateCategoryRequest Request) : IRequest<CategoryResponse>;

public class CreateCategoryCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext)
    : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var exist = await dbContext.Categories
            .AnyAsync(x => x.Name.ToLower() == request.Request.Name.ToLower(), 
                cancellationToken);

        if (exist)
        {
            throw new RecourseIsAlreadyExistException("Category already exists");
        }

        var category = mapper.Map<Models.Category>(request.Request);
        category.CategoryStatus = CategoryStatus.Active;
        await dbContext.Categories
            .AddAsync(category, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<CategoryResponse>(category);
    }
}