using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Purchase.Mapper;
using Pharmacy.CQRS.Purchase.Models.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Queries;

public record GetPurchaseByEmployeeIdQuery(
    long PharmacyId,
    int EmployeeId,
    int Page,
    int PageSize) : IRequest<List<PurchaseResponse>>;

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
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return PurchaseMappers.ToListPurchaseResponse(purchases);
    }
}