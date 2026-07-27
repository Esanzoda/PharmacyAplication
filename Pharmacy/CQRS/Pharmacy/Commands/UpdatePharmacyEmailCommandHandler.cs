using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Pharmacy.Commands;

public record UpdatePharmacyEmailCommand(long Id, string NewEmail) : IRequest<PharmacyResponse>;

public class UpdatePharmacyEmailCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<UpdatePharmacyEmailCommand, PharmacyResponse>
{
    public async Task<PharmacyResponse> Handle(UpdatePharmacyEmailCommand request, CancellationToken cancellationToken)
    {
        var pharmacy = await dbContext.Pharmacies
            .FindAsync(request.Id, cancellationToken);
        if (pharmacy is null)
        {
            throw new RecourseNotFoundException("Pharmacy not found");
        }

        var pharmacyExist = await dbContext.Pharmacies
            .AnyAsync(x => x.Id != request.Id &&
                           x.Email == request.NewEmail,
                cancellationToken);
        if (pharmacyExist)
        {
            throw new RecourseIsAlreadyExistException("Pharmacy with this email already exist");
        }

        pharmacy.Email = request.NewEmail;
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}