using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Deliver.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;

namespace PharmacyService.Api.Application.CQRS.Deliver.Commands;

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
        
        return DeliverMappers.ToDeliverResponse(deliver);
    }
}