using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Purchase.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;

namespace PharmacyService.Api.Application.CQRS.Purchase.Queries;

public record GetPurchaseByEmployeeIdQuery(
    long PharmacyId,
    int EmployeeId,
    PaginationRequest PaginationRequest) : IRequest<List<PurchaseResponse>>;

public class GetPurchaseByEmployeeIdQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetPurchaseByEmployeeIdQuery, List<PurchaseResponse>>
{
    public async Task<List<PurchaseResponse>> Handle(
        GetPurchaseByEmployeeIdQuery request,
        CancellationToken cancellationToken)
    {
        var purchases = await dbContext.Purchases
            .AsNoTracking()
            .Include(x => x.PurchaseItems)
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.EmployeeEntityId == request.EmployeeId)
            .OrderBy(o => o.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return PurchaseMappers.ToListPurchaseResponse(purchases);
    }
}