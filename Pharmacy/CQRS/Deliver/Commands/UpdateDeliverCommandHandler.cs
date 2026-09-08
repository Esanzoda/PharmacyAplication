using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Deliver.Mapper;
using Pharmacy.Domain.Models.Deliver.DTOs.Request;
using Pharmacy.Domain.Models.Deliver.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Deliver.Commands;

public record UpdateDeliverCommand(
    long Id,
    UpdateDeliverRequest Request) : IRequest<DeliverResponse>;

public class UpdateDeliverHandler(
    IApplicationDbContext dbContext) : IRequestHandler<UpdateDeliverCommand, DeliverResponse>
{
    public async Task<DeliverResponse> Handle(
        UpdateDeliverCommand request,
        CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FindAsync([request.Id],
                cancellationToken);

        if (deliver is null)
        {
            throw new ResourceNotFoundException("Deliver not found");
        }

        var deliverExist = await dbContext.Delivers
            .AnyAsync(
                x => x.Id != request.Id &&
                     (x.Email == request.Request.Email ||
                      x.PhoneNumber == request.Request.PhoneNumber),
                cancellationToken);

        if (deliverExist)
        {
            throw new BusinessException("Deliver with this number or email  already exists");
        }

        DeliverMappers.ToDeliver(deliver, request.Request);
        await dbContext.SaveChangesAsync(cancellationToken);
        return DeliverMappers.ToDeliverResponse(deliver);
    }
}