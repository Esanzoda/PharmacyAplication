using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Pharmacy.Commands;

public record UpdatePharmacyNameCommand(
    long Id,
    string NewName) : IRequest<PharmacyResponse>;

public class UpdatePharmacyNameCommandHandler(IMapper mapper, IApplicationDbContext dbContext)
    : IRequestHandler<UpdatePharmacyNameCommand, PharmacyResponse>
{
    public async Task<PharmacyResponse> Handle(UpdatePharmacyNameCommand request, CancellationToken cancellationToken)
    {
        var pharmacy = await dbContext.Pharmacies
            .FindAsync(request.Id, cancellationToken);
        if (pharmacy is null)
        {
            throw new RecourseNotFoundException("Pharmacy not found");
        }

        var pharmacyExist = await dbContext.Pharmacies
            .AnyAsync(x => x.Id != request.Id &&
                           x.Name == request.NewName &&
                           x.Address == pharmacy.Address,
                cancellationToken);
        if (pharmacyExist)
        {
            throw new RecourseIsAlreadyExistException("Pharmacy with this name already exist");
        }

        pharmacy.Name = request.NewName;
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}