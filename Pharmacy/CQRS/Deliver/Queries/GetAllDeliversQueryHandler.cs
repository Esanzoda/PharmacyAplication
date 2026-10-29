using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Deliver.Mapper;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Deliver.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Deliver.Queries;

public record GetAllDeliverQuery(
    PaginationRequest PaginationRequest) : IRequest<List<DeliverResponse>>;

public class GetAllDeliversQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetAllDeliverQuery, List<DeliverResponse>>
{
    public async Task<List<DeliverResponse>> Handle(
        GetAllDeliverQuery request,
        CancellationToken cancellationToken)
    {
        var delivers = await dbContext.Delivers
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return DeliverMappers.ToListDeliverResponse(delivers);
    }
}