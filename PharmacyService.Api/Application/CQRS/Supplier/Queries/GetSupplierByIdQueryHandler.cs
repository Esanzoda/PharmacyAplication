using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Supplier.Mappers;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;

namespace PharmacyService.Api.Application.CQRS.Supplier.Queries;

public record GetSupplierByIdQuery(
    long Id) : IRequest<SupplierResponse>;

public class GetSupplierByIdQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetSupplierByIdQuery, SupplierResponse>
{
    public async Task<SupplierResponse> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var supplier = await dbContext.Suppliers
            .FirstOrDefaultAsync(x => x.Id == request.Id,
                cancellationToken);

        if (supplier == null)
        {
            throw new ResourceNotFoundException("Supplier not found");
        }

        return SupplierMappers.ToSupplierResponse(supplier);
    }
}