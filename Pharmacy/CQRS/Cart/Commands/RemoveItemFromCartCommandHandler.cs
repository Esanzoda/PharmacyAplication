using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Cart.Commands;

public record RemoveItemFromCartCommand(
    long CustomerId,
    long ProductId) : IRequest<CartResponse>;

public class RemoveItemFromCartCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<RemoveItemFromCartCommand, CartResponse>
{
    public async Task<CartResponse> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId, cancellationToken);

        if (cart is null)
        {
            throw new RecourseNotFoundException("Cart not found");
        }

        var item = cart.CartItems.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (item is null)
        {
            throw new RecourseNotFoundException("Cart item not found");
        }

        cart.CartItems.Remove(item);
        dbContext.CartItems.Remove(item);

        cart.TotalAmount = cart.CartItems.Sum(x => x.TotalPrice);

        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<CartResponse>(cart);
    }
}