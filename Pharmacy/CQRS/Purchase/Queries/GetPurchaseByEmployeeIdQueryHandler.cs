using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Purchase.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Purchase.Queries;

public record GetPurchaseByEmployeeIdQuery(
    long PharmacyId,
    int EmployeeId,
    int Page,
    int PageSize) : IRequest<List<PurchaseResponse>>;

public class GetPurchaseByEmployeeIdQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper)
    : IRequestHandler<GetPurchaseByEmployeeIdQuery, List<PurchaseResponse>>
{
    public async Task<List<PurchaseResponse>> Handle(GetPurchaseByEmployeeIdQuery request,
        CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .Include(x => x.PurchaseItems)
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.EmployeeId == request.EmployeeId)
            .OrderBy(o => o.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        if (purchase == null)
        {
            throw new RecourseNotFoundException("Purchase not found");
        }

        return mapper.Map<List<PurchaseResponse>>(purchase);
    }
}