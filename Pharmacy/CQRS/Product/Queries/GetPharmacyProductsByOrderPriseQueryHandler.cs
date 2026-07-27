using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Queries;

public record GetPharmacyProductsByOrderPriceQuery(
    long PharmacyId,
    decimal Price,
    int Page,
    int PageSize) : IRequest<List<ProductResponse>>;

public class GetPharmacyProductsByOrderPriseQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetPharmacyProductsByOrderPriceQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetPharmacyProductsByOrderPriceQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Price == request.Price)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (!product.Any())
            throw new RecourseNotFoundException("Product with this price  not found");

        return mapper.Map<List<ProductResponse>>(product);
    }
}