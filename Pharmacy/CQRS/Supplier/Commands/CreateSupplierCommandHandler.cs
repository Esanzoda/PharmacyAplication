using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Supplier.Mappers;
using Pharmacy.Domain.Models.Deliver;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Services.GoogleMaps;
using Pharmacy.Services.Password;

namespace Pharmacy.CQRS.Supplier.Commands;

public record CreateSupplierCommand(
    SupplierRequest Request) : IRequest<SupplierResponse>;

public class CreateSupplierCommandHandler(
    IApplicationDbContext dbContext,
    IPasswordService passwordService,
    IGeocodingService geocodingService) : IRequestHandler<CreateSupplierCommand, SupplierResponse>
{
    public async Task<SupplierResponse> Handle(CreateSupplierCommand request,
        CancellationToken cancellationToken)
    {
        var supplierExists = await dbContext.Suppliers
            .AnyAsync(x => x.Email == request.Request.Email.ToLower() ||
                           x.VehicleNumber == request.Request.VehicleNumber.ToUpper(),
                cancellationToken);

        if (supplierExists)
        {
            throw new ResourceIsAlreadyExistException("Supplier already exists");
        }

        await geocodingService.GetCoordinatesAsync(request.Request.Address);
        var passwordHash = await passwordService.PasswordHash(request.Request.Password);

        var newSupplier = SupplierMappers.ToSupplier(request.Request);
        newSupplier.PasswordHash = passwordHash;

        await dbContext.Suppliers
            .AddAsync(newSupplier, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return SupplierMappers.ToSupplierResponse(newSupplier);
    }
}