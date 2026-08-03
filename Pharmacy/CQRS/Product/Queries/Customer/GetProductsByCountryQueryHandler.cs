using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Product.Queries.Customer;

public record GetProductsByCountryQuery(
    CountryEnum Country,
    int Page,
    int PageSize) : IRequest<List<ProductResponse>>;

public class GetProductsByCountryQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetProductsByCountryQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetProductsByCountryQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .Where(x => x.Country == request.Country)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (!products.Any())
        {
            throw new RecourseNotFoundException($"Product from this country[{request.Country}] not found");
        }

        return mapper.Map<List<ProductResponse>>(products);
    }
}