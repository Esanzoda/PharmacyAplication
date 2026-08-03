using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Cart.Queries;

public record GetCartByCustomerIdQuery(
    long CustomerId) : IRequest<CartResponse>;

public class GetCartByCustomerIdQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<GetCartByCustomerIdQuery, CartResponse>
{
    public async Task<CartResponse> Handle(GetCartByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.CartItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId,
                cancellationToken);
        if (cart is null)
        {
            throw new RecourseNotFoundException("Cart not found");
        }

        return mapper.Map<CartResponse>(cart);
    }
}