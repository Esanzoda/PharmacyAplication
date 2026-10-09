using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Product.DTos.Request;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Pharmacy.Exception;
using Product.CQRS.Product.Mappers;
using Product.Interfaces;

namespace Product.CQRS.Product.Commands;

public record UpdateProductCommand(
    long PharmacyId,
    long Id,
    UpdateProductRequest Request) : IRequest<ProductForPharmacyResponse>;

public class UpdateProductCommandHandler(IProductDbContext dbContext) :
    IRequestHandler<UpdateProductCommand, ProductForPharmacyResponse>
{
    public async Task<ProductForPharmacyResponse> Handle(UpdateProductCommand request,
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

        var productExist = await dbContext.Products
            .AnyAsync(x => x.PharmacyId == request.PharmacyId &&
                           x.Name == request.Request.Name &&
                           x.ProductType == request.Request.ProductType,
                cancellationToken);
        if (productExist)
        {
            throw new ResourceIsAlreadyExistException(
                $"Product already exists with  information");
        }

        ProductMappers.ToProduct(product, request.Request);
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);

        }
        catch (Exception e)
        {
            Console.WriteLine($"AAAAA {e.Message}");
            Console.WriteLine($"AA {e.InnerException!.Message}");
            throw;
        }
        return ProductMappers.ToProductForPharmacyResponse(product);
    }
}