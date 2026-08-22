using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Mappers;
using Pharmacy.CQRS.Cart.Models.DTOs.Request;
using Pharmacy.CQRS.Cart.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Cart.Commands;

public record AddItemToCartCommand(
    long CustomerId,
    CartItemRequest ItemRequest) : IRequest<CartResponse>;

public class AddItemToCartCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<AddItemToCartCommand, CartResponse>
{
    public async Task<CartResponse> Handle(
        AddItemToCartCommand request,
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

        var product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == request.ItemRequest.ProductId &&
                                      x.IsDeleted == false,
                cancellationToken);
        if (product == null)
        {
            throw new ResourceNotFoundException("Product not found");
        }

        var existingCartItem = cart.CartItems
            .FirstOrDefault(x => x.ProductEntityId == request.ItemRequest.ProductId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += request.ItemRequest.Quantity;
            existingCartItem.TotalPrice = existingCartItem.Quantity * existingCartItem.SalePrice;
        }
        else
        {
            var cartItem = CartMappers.ToCartItem(request.ItemRequest);
            cartItem.CustomerEntityId = request.CustomerId;
            cartItem.ProductEntityId = request.ItemRequest.ProductId;
            cartItem.CartEntity = cart;
            cartItem.SalePrice = product.SalePrice;
            cartItem.TotalPrice = cartItem.SalePrice * cartItem.Quantity;

            cart.CartItems.Add(cartItem);
        }

        cart.TotalAmount = cart.CartItems.Sum(x => x.TotalPrice);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CartMappers.ToCartResponse(cart);
    }
}