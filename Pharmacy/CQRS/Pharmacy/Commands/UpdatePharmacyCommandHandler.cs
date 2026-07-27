using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Pharmacy.Commands;

public record UpdatePharmacyCommand(
    long Id,
    PharmacyRequest Request) : IRequest<PharmacyResponse>;

public class UpdatePharmacyCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<UpdatePharmacyCommand, PharmacyResponse>
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
            .AnyAsync(x => x.Email == request.Request.Email ||
                           x.PhoneNumber == request.Request.PhoneNumber  ||
                           (
                               x.Name == request.Request.Name &&
                               x.Address == request.Request.Address
                           )
                , cancellationToken);

        if (pharmacyExists)
        {
            throw new RecourseIsAlreadyExistException("Pharmacy with this information already exist");
        }
        

        mapper.Map(request.Request, pharmacy);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}