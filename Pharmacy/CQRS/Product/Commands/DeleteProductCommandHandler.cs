using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Commands;

public record DeleteProductCommand(
    long PharmacyId,
    long Id) : IRequest<bool>;

public class DeleteProductCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Include(x => x.ProductBatches)
            .FirstOrDefaultAsync(x => x.Id == request.Id &&
                                      x.PharmacyId == request.PharmacyId,
                cancellationToken);
        if (product is null)
        {
            throw new ResourceNotFoundException($"Product with id {request.Id} not found");
        }

        var productBatches = product.ProductBatches
            .Where(x => x.IsActive).ToList();
        foreach (var productBatch in productBatches)
        {
            productBatch.IsActive = false;
        }

        dbContext.Products.Remove(product);

        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}