using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Customer.Queries;

public record GetCustomerByNameQuery(
    string Name) : IRequest<List<CustomerResponse>>;

public class GetCustomerByNameQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext
) : IRequestHandler<GetCustomerByNameQuery, List<CustomerResponse>>
{
    public async Task<List<CustomerResponse>> Handle(GetCustomerByNameQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .Where(x => x.Name.ToLower().Contains(request.Name.ToLower()))
            .ToListAsync(cancellationToken);
        if (customer is null)
        {
            throw new RecourseNotFoundException($"Customer with this name{request.Name} not found");
        }

        return mapper.Map<List<CustomerResponse>>(customer);
    }
}