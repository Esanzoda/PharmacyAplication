using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Request;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Services.GoogleMaps;

namespace Pharmacy.CQRS.Pharmacy.Commands;

public record CreatePharmacyCommand(
    PharmacyRequest Request) : IRequest<PharmacyResponse>;

public class CreatePharmacyCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext,
    IGeocodingService geocodingService) : IRequestHandler<CreatePharmacyCommand, PharmacyResponse>
{
    public async Task<PharmacyResponse> Handle(CreatePharmacyCommand request, CancellationToken cancellationToken)
    {
        var pharmacyExists = await dbContext.Pharmacies
            .AnyAsync(x => x.Email == request.Request.Email ||
                           x.PhoneNumber == request.Request.PhoneNumber ||
                           (
                               x.Name == request.Request.Name &&
                               x.Address == request.Request.Address
                           )
                , cancellationToken);

        if (pharmacyExists)
        {
            throw new RecourseIsAlreadyExistException("Pharmacy with this information already exist");
        }

        var pharmacy = mapper.Map<Models.Pharmacy>(request.Request);
        var geoCoding = await geocodingService.GetCoordinatesAsync(pharmacy.Address);
        if (geoCoding == null)
        {
            throw new BusinessException("Cannot found coordinates for this address");
        }

        pharmacy.Latitude = geoCoding.Lat;
        pharmacy.Longitude = geoCoding.Lng;

        await dbContext.Pharmacies
            .AddAsync(pharmacy, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}