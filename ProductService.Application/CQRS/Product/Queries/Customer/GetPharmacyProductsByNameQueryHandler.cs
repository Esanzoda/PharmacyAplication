using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.CQRS.Product.Mappers;
using ProductService.Application.DTOs.Request;
using ProductService.Application.DTOs.Response;

namespace ProductService.Application.CQRS.Product.Queries.Customer;

public record GetPharmacyProductsByNameQuery(
    long PharmacyId,
    string ProductName,
    PaginationRequest PaginationRequest) : IRequest<List<ProductForCustomerResponse>>;

public class GetPharmacyProductsByNameQueryHandler(
    IProductDbContext dbContext) : IRequestHandler<GetPharmacyProductsByNameQuery, List<ProductForCustomerResponse>>
{
    public async Task<List<ProductForCustomerResponse>> Handle(
        GetPharmacyProductsByNameQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Name.ToLower().Contains(request.ProductName.ToLower()))
            .AsNoTracking()
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);
        
        return ProductMappers.ToListProductForCustomerResponse(products);
    }
    
}