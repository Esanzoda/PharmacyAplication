using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.Mapper;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries.Customer;

public record GetPharmacyProductsByNameQuery(
    long PharmacyId,
    string ProductName,
    int PageNumber,
    int PageSize) : IRequest<List<ProductForCustomerResponse>>;

public class GetPharmacyProductsByNameQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetPharmacyProductsByNameQuery, List<ProductForCustomerResponse>>
{
    public async Task<List<ProductForCustomerResponse>> Handle(
        GetPharmacyProductsByNameQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Name.ToLower().Contains(request.ProductName.ToLower()))
            .AsNoTracking()
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return ProductMappers.ToListProductForCustomerResponse(products);
    }
}