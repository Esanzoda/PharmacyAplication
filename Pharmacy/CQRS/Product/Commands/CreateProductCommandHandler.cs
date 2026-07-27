using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Commands;

public record CreateProductCommand(
    long PharmacyId,
    ProductRequest Request
) : IRequest<ProductResponse>;

public class CreateProductCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<CreateProductCommand, ProductResponse>
{
    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .AnyAsync(x => x.Id == request.Request.CategoryId, cancellationToken);
        if (!category)
        {
            throw new RecourseNotFoundException($"Category with this[{request.Request.CategoryId}] not found");
        }

        var productExist = await dbContext.Products
            .AnyAsync(x => x.PharmacyId == request.PharmacyId &&
                           x.Barcode == request.Request.Barcode, cancellationToken);
        if (productExist)
        {
            throw new RecourseIsAlreadyExistException(
                $"Product already exists with this name {request.Request.Name} or barcode {request.Request.Barcode}");
        }

        var product = mapper.Map<Models.Domain.Product>(request.Request);
        product.PharmacyId = request.PharmacyId;
        await dbContext.Products
            .AddAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<ProductResponse>(product);
    }
}