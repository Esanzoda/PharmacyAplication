using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Supplier.Mappers;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;

namespace PharmacyService.Api.Application.CQRS.Supplier.Commands;

public record UpdateSupplierCommand(
    long Id,
    UpdateSupplierRequest UpdateSupplierRequest) : IRequest<SupplierResponse>;

public class UpdateSupplierCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<UpdateSupplierCommand, SupplierResponse>
{
    public async Task<SupplierResponse> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await dbContext.Suppliers
            .FirstOrDefaultAsync(x => x.Id == request.Id,
                cancellationToken);

        if (supplier == null)
        {
            throw new ResourceNotFoundException("Suppler not found");
        }

        var supplierExists = await dbContext.Suppliers
            .AnyAsync(x => x.Email == request.UpdateSupplierRequest.Email.ToLower() &&
                           x.PhoneNumber == request.UpdateSupplierRequest.PhoneNumber &&
                           x.VehicleNumber == request.UpdateSupplierRequest.VehicleNumber,
                cancellationToken);

        if (supplierExists)
        {
            throw new ResourceIsAlreadyExistException("Supplier already exist with this information");
        }

        SupplierMappers.ToUpdateSupplier(request.UpdateSupplierRequest, supplier);
        await dbContext.SaveChangesAsync(cancellationToken);

        return SupplierMappers.ToSupplierResponse(supplier);
    }
}