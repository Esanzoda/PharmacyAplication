using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Mappers;
using Pharmacy.Domain.Models.Cart.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Cart.Commands;

public record RemoveItemFromCartCommand(
    long CustomerId,
    long ProductId) : IRequest<CartResponse>;

public class RemoveItemFromCartCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<RemoveItemFromCartCommand, CartResponse>
{
    public async Task<CartResponse> Handle(
        RemoveItemFromCartCommand request,
        CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.CustomerEntityId == request.CustomerId,
                cancellationToken);

        if (cart is null)
        {
            throw new ResourceNotFoundException("Cart not found");
        }

        var item = cart.CartItems.FirstOrDefault(x => x.ProductEntityId == request.ProductId);
        if (item is null)
        {
            throw new ResourceNotFoundException("Cart item not found");
        }

        cart.CartItems.Remove(item);
        dbContext.CartItems.Remove(item);

        cart.TotalAmount = cart.CartItems.Sum(x => x.TotalPrice);

        await dbContext.SaveChangesAsync(cancellationToken);
        return CartMappers.ToCartResponse(cart);
    }
}