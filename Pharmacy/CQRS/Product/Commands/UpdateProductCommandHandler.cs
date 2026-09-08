using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.Mapper;
using Pharmacy.Domain.Models.Product.DTos.Request;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Commands;

public record UpdateProductCommand(
    long PharmacyId,
    long Id,
    UpdateProductRequest Request) : IRequest<ProductForPharmacyResponse>;

public class UpdateProductCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<UpdateProductCommand, ProductForPharmacyResponse>
{
    public async Task<ProductForPharmacyResponse> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Id,
                cancellationToken);
        if (product == null)
        {
            throw new ResourceNotFoundException($"Product with this id {request.Id} not found");
        }

        var categoryExists = await dbContext.Categories
            .AnyAsync(x => x.Id == request.Request.CategoryId,
                cancellationToken);
        if (!categoryExists)
        {
            throw new ResourceNotFoundException($"Category with this id {request.Request.CategoryId} not found");
        }

        var productExist = await dbContext.Products
            .AnyAsync(x => x.PharmacyId == request.PharmacyId &&
                           x.Id != request.Id &&
                           x.Barcode == product.Barcode,
                cancellationToken);
        if (productExist)
        {
            throw new ResourceIsAlreadyExistException(
                $"Product already exists with  Barcode {product.Barcode} ");
        }

        product = ProductMappers.ToProduct(request.Request);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ProductMappers.ToProductForPharmacyResponse(product);
    }
}