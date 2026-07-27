using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Queries;

public record GetPharmacyProductsByNameQuery(
    long PharmacyId,
    string Name,
    int Page,
    int PageSize) : IRequest<List<ProductResponse>>;

public class GetPharmacyProductsByNameQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetPharmacyProductsByNameQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetPharmacyProductsByNameQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Name.Contains(request.Name))
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (!product.Any())
            throw new RecourseNotFoundException("Product with this name not found");

        return mapper.Map<List<ProductResponse>>(product);
    }
}