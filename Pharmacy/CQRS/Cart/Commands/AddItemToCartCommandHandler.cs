using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Models;
using Pharmacy.CQRS.Cart.Models.DTOs.Request;
using Pharmacy.CQRS.Cart.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Cart.Commands;

public record AddItemToCartCommand(
    long CustomerId,
    CartItemRequest ItemRequest) : IRequest<CartResponse>;

public class AddItemToCartCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext)
    : IRequestHandler<AddItemToCartCommand, CartResponse>
{
    public async Task<CartResponse> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId,
                cancellationToken);

        if (cart is null)
        {
            throw new RecourseNotFoundException("Cart not found");
        }

        var product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == request.ItemRequest.ProductId &&
                                      x.IsDeleted == false,
                cancellationToken);
        if (product == null)
        {
            throw new RecourseNotFoundException("Product not found");
        }

        var existingCartItem = cart.CartItems
            .FirstOrDefault(x => x.ProductId == request.ItemRequest.ProductId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += request.ItemRequest.Quantity;
            existingCartItem.TotalPrice = existingCartItem.Quantity * existingCartItem.Price;
        }
        else
        {
            var cartItem = mapper.Map<CartItem>(request.ItemRequest);
            cartItem.CustomerId = request.CustomerId;
            cartItem.Cart = cart;
            cartItem.Price = product.SalePrice;
            cartItem.TotalPrice = cartItem.Price * cartItem.Quantity;

            cart.CartItems.Add(cartItem);
        }

        cart.TotalAmount = cart.CartItems.Sum(x => x.TotalPrice);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<CartResponse>(cart);
    }
}