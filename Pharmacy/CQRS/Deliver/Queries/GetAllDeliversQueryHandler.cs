using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Deliver.Mapper;
using Pharmacy.Domain.Models.Deliver.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Deliver.Queries;

public record GetAllDeliverQuery(
    int PageNumber,
    int PageSize) : IRequest<List<DeliverResponse>>;

public class GetAllDeliversQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetAllDeliverQuery, List<DeliverResponse>>
{
    public async Task<List<DeliverResponse>> Handle(
        GetAllDeliverQuery request,
        CancellationToken cancellationToken)
    {
        var delivers = await dbContext.Delivers
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return DeliverMappers.ToListDeliverResponse(delivers);
    }
}