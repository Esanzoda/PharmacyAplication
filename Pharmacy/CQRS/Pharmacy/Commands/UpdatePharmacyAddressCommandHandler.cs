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

        var geoCoding = await geocodingService.GetCoordinatesAsync(pharmacy.Address);

        pharmacy.Latitude = geoCoding.Lat;
        pharmacy.Longitude = geoCoding.Lng;

        pharmacy.Address = request.NewAddress;
        await dbContext.SaveChangesAsync(cancellationToken);

        return PharmacyMappers.ToPharmacyResponse(pharmacy);
    }
}