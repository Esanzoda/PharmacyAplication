using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Pharmacy.Mapper;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Pharmacy.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Pharmacy.Queries;

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