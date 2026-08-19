using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Employee.Models;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Request;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Services.GoogleMaps;
using Pharmacy.Services.Password;

namespace Pharmacy.CQRS.Pharmacy.Commands;

public record CreatePharmacyCommand(
    PharmacyRequest Request) : IRequest<PharmacyResponse>;

public class CreatePharmacyCommandHandler(
    IMapper mapper,
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

        var pharmacy = mapper.Map<Models.PharmacyEntity>(request.Request);
        var geoCoding = await geocodingService.GetCoordinatesAsync(pharmacy.Address);

        if (geoCoding == null)
        {
            throw new BusinessException("Cannot found coordinates for this address");
        }

        pharmacy.Latitude = geoCoding.Lat;
        pharmacy.Longitude = geoCoding.Lng;
        var password = "string";

        await dbContext.Pharmacies
            .AddAsync(pharmacy, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        var employee = new EmployeeEntity
        {
            Name = "AdminPharmacy",
            Address = pharmacy.Address,
            Email = pharmacy.Email,
            Role = Role.Employee,
            PasswordHash = await passwordService.PasswordHash(password),
            Position = Position.AdminPharmacy,
            Salary = 0,
            PharmacyId = pharmacy.Id
        };
        await dbContext.Employees
            .AddAsync(employee, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}