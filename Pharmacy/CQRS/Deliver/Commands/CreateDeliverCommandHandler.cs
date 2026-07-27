using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Deliver.Commands;

public record CreateDeliverCommand(
    long PharmacyId,
    DeliverRequest Request) : IRequest<DeliverResponse>;

public class CreateDeliverCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<CreateDeliverCommand, DeliverResponse>
{
    public async Task<DeliverResponse> Handle(CreateDeliverCommand request, CancellationToken cancellationToken)
    {
        var deliverExists = await dbContext.Delivers
            .AnyAsync(x => x.PharmacyId == request.PharmacyId &&
                           (x.Email == request.Request.Email ||
                            x.PhoneNumber == request.Request.PhoneNumber), cancellationToken);

        if (deliverExists)
        {
            throw new RecourseIsAlreadyExistException("Deliver already exist");
        }

        var newDeliver = mapper.Map<Models.Domain.Deliver>(request.Request);
        newDeliver.PharmacyId = request.PharmacyId;
        await dbContext.Delivers
            .AddAsync(newDeliver, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<DeliverResponse>(newDeliver);
    }
}