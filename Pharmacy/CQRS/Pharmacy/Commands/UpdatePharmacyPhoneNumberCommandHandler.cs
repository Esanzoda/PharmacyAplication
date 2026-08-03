using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Pharmacy.Commands;

public record UpdatePharmacyPhoneNumberCommand(
    long Id,
    string NewNumber) : IRequest<PharmacyResponse>;

public class UpdatePharmacyPhoneNumberCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<UpdatePharmacyPhoneNumberCommand, PharmacyResponse>
{
    public async Task<PharmacyResponse> Handle(UpdatePharmacyPhoneNumberCommand request,
        CancellationToken cancellationToken)
    {
        var pharmacy = await dbContext.Pharmacies
            .FindAsync(request.Id, cancellationToken);

        if (pharmacy is null)
        {
            throw new RecourseNotFoundException("Pharmacy not found.");
        }

        var pharmacyExist = await dbContext.Pharmacies
            .AnyAsync(x => x.Id != request.Id
                           && x.PhoneNumber == request.NewNumber,
                cancellationToken);
        if (pharmacyExist)
        {
            throw new RecourseIsAlreadyExistException(" Pharmacy with this number already exist ");
        }

        pharmacy.PhoneNumber = request.NewNumber;
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}