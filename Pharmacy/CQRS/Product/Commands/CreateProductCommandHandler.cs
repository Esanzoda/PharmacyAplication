using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.Mapper;
using Pharmacy.CQRS.Product.ProductModels.DTos.Request;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Commands;

public record CreateProductCommand(
    long PharmacyId,
    ProductRequest Request) : IRequest<ProductForPharmacyResponse>;

public class CreateProductCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<CreateProductCommand, ProductForPharmacyResponse>
{
    public async Task<ProductForPharmacyResponse> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .FindAsync([request.Request.CategoryId],
                cancellationToken);

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