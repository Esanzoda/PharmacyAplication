using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Pharmacy.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;

namespace PharmacyService.Api.Application.CQRS.Pharmacy.Queries;

public record GetAllPharmacyQuery(
    PaginationRequest PaginationRequest) : IRequest<List<PharmacyResponse>>;

public class GetAllPharmacyQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetAllPharmacyQuery, List<PharmacyResponse>>
{
    public async Task<List<PharmacyResponse>> Handle(
        GetAllPharmacyQuery request,
        CancellationToken cancellationToken)
    {
        var pharmacies = await dbContext.Pharmacies
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return PharmacyMappers.ToListPharmacyResponse(pharmacies);
    }
}