using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Pharmacy.Mapper;
using Pharmacy.Domain.Models.Pharmacy.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Services.GoogleMaps;

namespace Pharmacy.CQRS.Pharmacy.Commands;

public record UpdatePharmacyAddressCommand(
    long Id,
    string NewAddress) : IRequest<PharmacyResponse>;

public class UpdatePharmacyAddressCommandHandler(
    IApplicationDbContext dbContext,
    IGeocodingService geocodingService) : IRequestHandler<UpdatePharmacyAddressCommand, PharmacyResponse>
{
    public async Task<PharmacyResponse> Handle(
        UpdatePharmacyAddressCommand request,
        CancellationToken cancellationToken)
    {
        var pharmacy = await dbContext.Pharmacies
            .FindAsync([request.Id],
                cancellationToken);

        if (pharmacy is null)
        {
            throw new ResourceNotFoundException("Pharmacy not found");
        }

        var pharmacyExists = await dbContext.Pharmacies
            .AnyAsync(x => x.Id != request.Id &&
                           x.Address.ToLower() == request.NewAddress.ToLower(),
                cancellationToken);

        if (pharmacyExists)
        {
            throw new ResourceIsAlreadyExistException("Pharmacy with this address already exist");
        }

        var bossPharmacy = await dbContext.BossPharmacies
            .FirstOrDefaultAsync(x => x.PharmacyId == pharmacy.Id
                , cancellationToken);

        if (bossPharmacy is null)
        {
            throw new ResourceNotFoundException("BossPharmacy not found");
        }

        var geoCoding = await geocodingService.GetCoordinatesAsync(request.NewAddress);

        pharmacy.Address = request.NewAddress;
        pharmacy.Latitude = geoCoding.Lat;
        pharmacy.Longitude = geoCoding.Lng;
        bossPharmacy.Address = pharmacy.Address;

        pharmacy.Address = request.NewAddress;
        await dbContext.SaveChangesAsync(cancellationToken);

        return PharmacyMappers.ToPharmacyResponse(pharmacy);
    }
}