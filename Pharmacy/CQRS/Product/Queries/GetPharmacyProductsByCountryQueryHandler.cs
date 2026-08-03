using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Queries;

public record GetPharmacyProductsByCountryQuery(
    long PharmacyId,
    CountryEnum Country,
    int Page,
    int PageSize) : IRequest<List<ProductResponse>>;

public class GetPharmacyProductsByCountryQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetPharmacyProductsByCountryQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetPharmacyProductsByCountryQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Country == request.Country)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (!products.Any())
            throw new RecourseNotFoundException($"Product from this country[{request.Country}] not found");
        return mapper.Map<List<ProductResponse>>(products);
    }
}