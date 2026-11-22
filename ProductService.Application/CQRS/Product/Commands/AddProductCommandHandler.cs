using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.CQRS.Product.Mappers;
using ProductService.Application.DTOs.Request;
using ProductService.Application.DTOs.Response;
using ProductService.Application.Exceptions;

namespace ProductService.Application.CQRS.Product.Commands;

public record AddProductCommand(
    long PharmacyId,
    ProductRequest Request) : IRequest<ProductForPharmacyResponse>;

public class AddProductCommandHandler(
    IProductDbContext dbContext
) : IRequestHandler<AddProductCommand, ProductForPharmacyResponse>
{
    public async Task<ProductForPharmacyResponse> Handle(AddProductCommand request,
        CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .FindAsync([request.Request.CategoryId], cancellationToken);
        if (category == null)
        {
            throw new ResourceNotFoundException($"Category with this[{request.Request.CategoryId}] not found");
        }

        var productExist = await dbContext.Products
            .AnyAsync(x => x.PharmacyId == request.PharmacyId &&
                           x.Barcode == request.Request.Barcode,
                cancellationToken);

        if (productExist)
        {
            throw new ResourceIsAlreadyExistException(
                $"Product already exists with  barcode {request.Request.Barcode}");
        }

        var product = ProductMappers.ToProduct(request.Request);
        product.PharmacyId = request.PharmacyId;
        product.CategoryEntity = category;

        await dbContext.Products.AddAsync(product,
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ProductMappers.ToProductForPharmacyResponse(product);
    }
}