using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Commands;

public record UpdateProductCommand(
    long PharmacyId,
    long Id,
    ProductRequest Request)
    : IRequest<ProductResponse>;

public class UpdateProductCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<UpdateProductCommand, ProductResponse>
{
    public async Task<ProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var categoryExists = await dbContext.Categories
            .AnyAsync(x => x.Id == request.Request.CategoryId,
                cancellationToken);
        if (!categoryExists)
        {
            throw new RecourseNotFoundException($"Category with this id {request.Request.CategoryId} not found");
        }

        var product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == request.Id &&
                                      x.PharmacyId == request.PharmacyId,
                cancellationToken);
        if (product == null)
        {
            throw new RecourseNotFoundException($"Product with this id {request.Id} not found");
        }

        var productExist = await dbContext.Products
            .AnyAsync(x => x.Id != request.Id &&
                           (x.Name == request.Request.Name &&
                            x.Barcode == request.Request.Barcode),
                cancellationToken);
        if (productExist)
        {
            throw new RecourseIsAlreadyExistException(
                $"Product already exists with Name {request.Request.Name} or with Barcode {request.Request.Barcode} ");
        }

        mapper.Map(request.Request, product);

        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<ProductResponse>(product);
    }
}