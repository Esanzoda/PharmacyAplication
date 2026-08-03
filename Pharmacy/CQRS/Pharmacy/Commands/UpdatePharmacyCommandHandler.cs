using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Request;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Services.GoogleMaps;

namespace Pharmacy.CQRS.Pharmacy.Commands;

public record UpdatePharmacyCommand(
    long Id,
    PharmacyRequest Request) : IRequest<PharmacyResponse>;

public class UpdatePharmacyCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext,
    IGeocodingService geocodingService) : IRequestHandler<UpdatePharmacyCommand, PharmacyResponse>
{
    public async Task<PharmacyResponse> Handle(UpdatePharmacyCommand request, CancellationToken cancellationToken)
    {
        var pharmacy = await dbContext.Pharmacies
            .FindAsync(request.Id, cancellationToken);
        if (pharmacy is null)
        {
            throw new RecourseNotFoundException("Pharmacy not found");
        }

        var pharmacyExists = await dbContext.Pharmacies
            .AnyAsync(x => x.Id != request.Id && (
                    x.Email == request.Request.Email ||
                    x.PhoneNumber == request.Request.PhoneNumber ||
                    (
                        x.Name == request.Request.Name &&
                        x.Address == request.Request.Address
                    ))
                , cancellationToken);

        if (pharmacyExists)
        {
            throw new RecourseIsAlreadyExistException("Pharmacy with this information already exist");
        }

        if (pharmacy.Address != request.Request.Address)
        {
            var coordinates = await geocodingService.GetCoordinatesAsync(request.Request.Address);

            pharmacy.Latitude = coordinates.Lat;
            pharmacy.Longitude = coordinates.Lng;
        }

        mapper.Map(request.Request, pharmacy);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}