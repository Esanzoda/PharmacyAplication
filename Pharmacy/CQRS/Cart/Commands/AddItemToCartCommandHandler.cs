using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Cart.Commands;

public record AddItemToCartCommand(
    long PharmacyId,
    long CustomerId,
    CartItemRequest ItemRequest
) : IRequest<CartResponse>;

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
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.ItemRequest.ProductId,
                cancellationToken);
        if (product == null)
        {
            throw new RecourseNotFoundException("Product not found");
        }

        var existingCartItem = cart.CartItems
            .FirstOrDefault(x => x.ProductId == request.ItemRequest.ProductId);


        var existQuantity = existingCartItem?.Quantity ?? 0;
        var totalRequestedQuantity = existQuantity + request.ItemRequest.Quantity;

        if (existingCartItem != null)
        {
            existingCartItem.Quantity = totalRequestedQuantity;
            existingCartItem.TotalPrice = totalRequestedQuantity * existingCartItem.Price;
        }
        else
        {
            var cartItem = mapper.Map<CartItem>(request.ItemRequest);
            cartItem.CustomerId = request.CustomerId;
            cartItem.Cart = cart;
            cartItem.Price = product.Price;
            cartItem.TotalPrice = product.Price * request.ItemRequest.Quantity;

            cart.CartItems.Add(cartItem);
        }

        cart.TotalAmount = cart.CartItems.Sum(x => x.TotalPrice);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<CartResponse>(cart);
    }
}