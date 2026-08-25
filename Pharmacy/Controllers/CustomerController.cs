using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Cart.Commands;
using Pharmacy.CQRS.Cart.Models.DTOs.Request;
using Pharmacy.CQRS.Cart.Queries;
using Pharmacy.CQRS.Category.Models.DTOs.Response;
using Pharmacy.CQRS.Category.Queries;
using Pharmacy.CQRS.Customer.Commands;
using Pharmacy.CQRS.Customer.Models.DTOs.Request;
using Pharmacy.CQRS.Customer.Models.DTOs.Response;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Response;
using Pharmacy.CQRS.Pharmacy.Queries;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response.Customer;
using Pharmacy.CQRS.Product.Queries.Customer;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Customer))]
public class CustomerController(IMediator mediator) : ControllerBase
{
    [HttpPatch]
    public async Task<IActionResult> UpdateCartItemQuantity(long productId, int quantity)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdateQuantityCartItemCommand(customerId, productId, quantity));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<CustomerResponse>> Update(UpdateCustomerRequest request)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdateCustomerCommand(customerId, request));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<CustomerResponse>> UpdatePassword(string oldPassword, string newPassword)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdateCustomerPasswordCommand(customerId, oldPassword, newPassword));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<string>> UpdateAddress(string newAddress)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdateCustomerAddressCommand(customerId, newAddress));
        return Ok(response);
    }


    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetCategoryWithProducts(int categoryId, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByCategoryIdQuery(categoryId, page, pageSize));
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCartAsync(CartItemRequest request)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new AddItemToCartCommand(customerId, request));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveItemFromCartAsync(long productId)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new RemoveItemFromCartCommand(customerId, productId));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCartAsync()
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new ClearCartCommand(customerId));
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetBiCustomerId()
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new GetCartByCustomerIdQuery(customerId));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<PharmacyResponse>> GetAllPharmacy(int page, int pageSize)
    {
        var response = await mediator.Send(new GetAllPharmacyQuery(page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllCategory(int page, int pageSize)
    {
        var response = await mediator.Send(new GetAllCategoriesByPaginationQuery(page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetPharmacyProductsByCategory(long pharmacyId,
        long categoryId, int page, int pageSize)
    {
        var response =
            await mediator.Send(new GetPharmacyProductsByCategoryIdQuery(pharmacyId, categoryId, page, pageSize));
        return Ok(response);
    }
}