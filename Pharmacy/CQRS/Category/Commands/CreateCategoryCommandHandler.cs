using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

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
            .AnyAsync(x => x.Name == request.Request.Name, 
                cancellationToken);

        if (exist)
        {
            throw new RecourseIsAlreadyExistException("Category already exist");
        }

        var category = mapper.Map<Models.Domain.Category>(request.Request);
        category.CategoryStatus = CategoryStatus.Active;
        await dbContext.Categories
            .AddAsync(category, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<CategoryResponse>(category);
    }
}