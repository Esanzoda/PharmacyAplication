using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Cart.Commands;
using Pharmacy.CQRS.Cart.Models.DTOs.Request;
using Pharmacy.CQRS.Cart.Queries;
using Pharmacy.CQRS.Category.Queries;
using Pharmacy.CQRS.Customer.Commands;
using Pharmacy.CQRS.Customer.Models.DTOs.Request;
using Pharmacy.CQRS.Customer.Models.DTOs.Response;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.CQRS.Product.Queries.Customer;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CustomerController(IMediator mediator) : ControllerBase
{
    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpPatch]
    public async Task<IActionResult> UpdateCartItemQuantity(long productId, int quantity)
    {
        var customerId = 1; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdateQuantityCartItemCommand(customerId, productId, quantity));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpPut]
    public async Task<ActionResult<CustomerResponse>> Update(UpdateCustomerRequest request)
    {
        var customerId = 1; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdateCustomerCommand(customerId, request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpPatch]
    public async Task<ActionResult<CustomerResponse>> UpdatePassword(string oldPassword, string newPassword)
    {
        var customerId = 1; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdateCustomerPasswordCommand(customerId, oldPassword, newPassword));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetCategoryWithProducts(int categoryId, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByCategoryIdQuery(categoryId, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpPost]
    public async Task<IActionResult> AddToCartAsync(CartItemRequest request)
    {
        var customerId = 4; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new AddItemToCartCommand(customerId, request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpDelete]
    public async Task<IActionResult> RemoveItemFromCartAsync(long productId)
    {
        var customerId = 1; // long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new RemoveItemFromCartCommand(customerId, productId));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpDelete]
    public async Task<IActionResult> ClearCartAsync()
    {
        var customerId = 4; // long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new ClearCartCommand(customerId));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpGet]
    public async Task<IActionResult> GetBiCustomerId()
    {
        var customerId = 1; // long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new GetCartByCustomerIdQuery(customerId));
        return Ok(response);
    }
}