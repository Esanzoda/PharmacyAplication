using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Pharmacy.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;
using PharmacyService.Api.Domain.Entities;
using PharmacyService.Api.Services.GoogleMaps;
using PharmacyService.Api.Services.Password;
using Role = PharmacyService.Api.Domain.Enums.Role;

namespace PharmacyService.Api.Application.CQRS.Pharmacy.Commands;

public record CreatePharmacyCommand(
    PharmacyRequest Request) : IRequest<PharmacyResponse>;

public class CreatePharmacyCommandHandler(
    IApplicationDbContext dbContext,
    IGeocodingService geocodingService,
    IPasswordService passwordService) : IRequestHandler<CreatePharmacyCommand, PharmacyResponse>
{
    public async Task<PharmacyResponse> Handle(
        CreatePharmacyCommand request,
        CancellationToken cancellationToken)
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
            throw new ResourceIsAlreadyExistException("Pharmacy with this information already exist");
        }

        var newPharmacy = PharmacyMappers.ToPharmacy(request.Request);
        var geoCoding = await geocodingService.GetCoordinatesAsync(newPharmacy.Address);

        if (geoCoding == null)
        {
            throw new BusinessException("Cannot found coordinates for this address");
        }

        newPharmacy.Latitude = geoCoding.Lat;
        newPharmacy.Longitude = geoCoding.Lng;
        var password = "string1234";

        await dbContext.Pharmacies
            .AddAsync(newPharmacy, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        var bossPharmacy = new BossPharmacy()
        {
            Name = "BossPharmacy",
            Address = newPharmacy.Address,
            Email = newPharmacy.Email,
            Role = Role.BossPharmacy,
            PasswordHash = await passwordService.PasswordHash(password),
            PharmacyId = newPharmacy.Id,
            PhoneNumber = newPharmacy.PhoneNumber
        };
        await dbContext.BossPharmacies
            .AddAsync(bossPharmacy, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return PharmacyMappers.ToPharmacyResponse(newPharmacy);
    }
}