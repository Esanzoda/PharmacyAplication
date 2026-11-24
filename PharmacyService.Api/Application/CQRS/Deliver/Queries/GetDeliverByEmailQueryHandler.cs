using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Deliver.Mapper;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;

namespace PharmacyService.Api.Application.CQRS.Deliver.Queries;

public record GetDeliverByEmailQuery(
    string Email) : IRequest<DeliverResponse>;

public class GetDeliverByEmailHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetDeliverByEmailQuery, DeliverResponse>
{
    public async Task<DeliverResponse> Handle(
        GetDeliverByEmailQuery request,
        CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FirstOrDefaultAsync(
                x => x.Email == request.Email &&
                     x.IsDeleted == false,
                cancellationToken);

        return deliver is null
            ? throw new ResourceNotFoundException("Deliver not found")
            : DeliverMappers.ToDeliverResponse(deliver);
    }
}