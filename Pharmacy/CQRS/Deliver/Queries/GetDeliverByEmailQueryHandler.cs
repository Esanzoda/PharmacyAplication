using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Controllers;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Deliver.Queries;

public record GetDeliverByEmailQuery(
    string Email) : IRequest<DeliverResponse>;

public class GetDeliverByEmailHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetDeliverByEmailQuery, DeliverResponse>
{
    public async Task<DeliverResponse> Handle(GetDeliverByEmailQuery request, CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FirstOrDefaultAsync(
                x => x.Email == request.Email,
                cancellationToken);
        if (deliver is null)
        {
            throw new RecourseNotFoundException("Deliver not found");
        }

        return mapper.Map<DeliverResponse>(deliver);
    }
}