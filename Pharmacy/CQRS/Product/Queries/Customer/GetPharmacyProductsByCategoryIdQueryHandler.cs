using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.Mapper;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries.Customer;

public record GetPharmacyProductsByCategoryIdQuery(
    long PharmacyId,
    long CategoryId,
    int PageNumber,
    int PageSize) : IRequest<List<ProductForCustomerResponse>>;

public class GetPharmacyProductsByCategoryIdQueryHandler(
    IApplicationDbContext dbContext)
    : IRequestHandler<GetPharmacyProductsByCategoryIdQuery, List<ProductForCustomerResponse>>
{
    public async Task<List<ProductForCustomerResponse>> Handle(
        GetPharmacyProductsByCategoryIdQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.CategoryEntityId == request.CategoryId)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForCustomerResponse(products);
    }
}