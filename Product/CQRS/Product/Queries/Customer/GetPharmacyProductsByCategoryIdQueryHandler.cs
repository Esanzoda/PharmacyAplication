using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Product.CQRS.Product.Mappers;
using Product.Interfaces;

namespace Product.CQRS.Product.Queries.Customer;

public record GetPharmacyProductsByCategoryIdQuery(
    long PharmacyId,
    long CategoryId,
    PaginationRequest PaginationRequest) : IRequest<List<ProductForCustomerResponse>>;

public class GetPharmacyProductsByCategoryIdQueryHandler(
    IProductDbContext dbContext)
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
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForCustomerResponse(products);
    }
}