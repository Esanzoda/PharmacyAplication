using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Deliver.Models.DTOs.Request;
using Pharmacy.CQRS.Deliver.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Deliver.Commands;

public record UpdateDeliverCommand(
    long Id,
    UpdateDeliverRequest Request) : IRequest<DeliverResponse>;

public class UpdateDeliverHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<UpdateDeliverCommand, DeliverResponse>
{
    public async Task<DeliverResponse> Handle(UpdateDeliverCommand request, CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (deliver is null)
        {
            throw new RecourseNotFoundException("Deliver not found");
        }

        var deliverExist = await dbContext.Delivers
            .AnyAsync(
                x => x.Id != request.Id &&
                     (x.Email == request.Request.Email ||
                      x.PhoneNumber == request.Request.PhoneNumber), cancellationToken);


        if (deliverExist)
        {
            throw new BusinessException("Deliver with this number or email  already exists");
        }

        mapper.Map(request.Request, deliver);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<DeliverResponse>(deliver);
    }
}