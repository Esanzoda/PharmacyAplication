using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Mappers;
using Pharmacy.CQRS.Cart.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Cart.Queries;

public record GetCartByCustomerIdQuery(
    long CustomerId) : IRequest<CartResponse>;

public class GetCartByCustomerIdQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetCartByCustomerIdQuery, CartResponse>
{
    public async Task<CartResponse> Handle(
        GetCartByCustomerIdQuery request,
        CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.CartItems)
            .ThenInclude(x => x.ProductEntity)
            .FirstOrDefaultAsync(x => x.CustomerEntityId == request.CustomerId,
                cancellationToken);

        return cart is null ? throw new ResourceNotFoundException("Cart not found") : CartMappers.ToCartResponse(cart);
    }
}