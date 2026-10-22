using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Supplier.Commands;

public record DeleteSupplierCommand(
    long Id) : IRequest<string>;

public class DeleteSupplierCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<DeleteSupplierCommand, string>
{
    public async Task<string> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await dbContext.Suppliers
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (supplier == null)
        {
            throw new ResourceNotFoundException("Supplier not found");
        }

        dbContext.Suppliers
            .Remove(supplier);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Message.Deleted;
    }
}