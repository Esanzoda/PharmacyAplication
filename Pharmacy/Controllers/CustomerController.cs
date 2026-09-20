using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Cart.Commands;
using Pharmacy.CQRS.Cart.Queries;
using Pharmacy.CQRS.Category.Queries;
using Pharmacy.CQRS.Customer.Commands;
using Pharmacy.CQRS.Pharmacy.Queries;
using Pharmacy.CQRS.Product.Queries.Customer;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Cart.DTOs.Request;
using Pharmacy.Domain.Models.Cart.DTOs.Response;
using Pharmacy.Domain.Models.Category.DTOs.Response;
using Pharmacy.Domain.Models.Customer.DTOs.Request;
using Pharmacy.Domain.Models.Customer.DTOs.Response;
using Pharmacy.Domain.Models.Pharmacy.DTOs.Response;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Customer))]
public class CustomerController(IMediator mediator) : ControllerBase
{
    [HttpPatch]
    public async Task<ActionResult<CartItemResponse>> UpdateCartItemQuantity(long customerId,long productId, int quantity)
    {
        var response = await mediator.Send(new UpdateQuantityCartItemCommand(customerId, productId, quantity));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<CustomerResponse>> Update(long customerId,UpdateCustomerRequest request)
    {
        var response = await mediator.Send(new UpdateCustomerCommand(customerId, request));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<CustomerResponse>> UpdatePassword(long customerId,string oldPassword, string newPassword)
    {
        var response = await mediator.Send(new UpdateCustomerPasswordCommand(customerId, oldPassword, newPassword));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<string>> UpdateAddress(long customerId,string newAddress)
    {
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
    public async Task<ActionResult<CartResponse>> AddToCart(long customerId,CartItemRequest request)
    {
        var response = await mediator.Send(new AddItemToCartCommand(customerId, request));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<CartResponse>> RemoveItemFromCart(long customerId,long productId)
    {
        var response = await mediator.Send(new RemoveItemFromCartCommand(customerId, productId));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<CartResponse>> ClearCart(long customerId)
    {
        var response = await mediator.Send(new ClearCartCommand(customerId));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CartResponse>> GetCartByCustomerId(long customerId)
    {
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
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetPharmacyProductsByCategoryId(long pharmacyId,
        long categoryId, int page, int pageSize)
    {
        var response =
            await mediator.Send(new GetPharmacyProductsByCategoryIdQuery(pharmacyId, categoryId, page, pageSize));
        return Ok(response);
    }
}