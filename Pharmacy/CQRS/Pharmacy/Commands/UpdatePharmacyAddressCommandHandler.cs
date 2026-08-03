using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Services.GoogleMaps;

namespace Pharmacy.CQRS.Pharmacy.Commands;

public record UpdatePharmacyAddressCommand(
    long Id,
    string NewAddress) : IRequest<PharmacyResponse>;

public class UpdatePharmacyAddressCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext,
    IGeocodingService geocodingService) : IRequestHandler<UpdatePharmacyAddressCommand, PharmacyResponse>
{
    public async Task<PharmacyResponse> Handle(UpdatePharmacyAddressCommand request,
        CancellationToken cancellationToken)
    {
        var pharmacy = await dbContext.Pharmacies
            .FindAsync(request.Id, cancellationToken);
        if (pharmacy is null)
        {
            throw new RecourseNotFoundException("Pharmacy not found");
        }

        var pharmacyExists = await dbContext.Pharmacies
            .AnyAsync(x => x.Id != request.Id &&
                           x.Address == request.NewAddress,
                cancellationToken);
        if (pharmacyExists)
        {
            throw new RecourseIsAlreadyExistException("Pharmacy with this address already exist");
        }

        var geoCoding = await geocodingService.GetCoordinatesAsync(pharmacy.Address);

        pharmacy.Latitude = geoCoding.Lat;
        pharmacy.Longitude = geoCoding.Lng;

        pharmacy.Address = request.NewAddress;
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}