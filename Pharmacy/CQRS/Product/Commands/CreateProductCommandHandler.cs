using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Request;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response.Customer;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Commands;

public record CreateProductCommand(
    long PharmacyId,
    ProductRequest Request) : IRequest<ProductForCustomerResponse>;

public class CreateProductCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<CreateProductCommand, ProductForCustomerResponse>
{
    public async Task<ProductForCustomerResponse> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == request.Request.CategoryId,
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

        var product = mapper.Map<ProductModels.ProductEntity>(request.Request);
        product.PharmacyId = request.PharmacyId;
        product.CategoryEntity = category;

        await dbContext.Products.AddAsync(product,
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProductForCustomerResponse>(product);
    }
}