using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Queries;

public record GetAllPharmacyProductsQuery(
    long PharmacyId,
    int Page,
    int PageSize) : IRequest<List<ProductResponse>>;

public class GetAllPharmacyProductsQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetAllPharmacyProductsQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetAllPharmacyProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .Where(x => x.PharmacyId == request.PharmacyId)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return mapper.Map<List<ProductResponse>>(products);
    }
}