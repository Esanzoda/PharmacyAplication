using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Purchase.Commands;
using Pharmacy.CQRS.Purchase.Models.DTOs.Request;
using Pharmacy.CQRS.Purchase.Models.DTOs.Response;
using Pharmacy.CQRS.Purchase.Queries;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PurchaseController(IMediator mediator) : ControllerBase
{
    // [Authorize(Roles = nameof(Role.PurchasingManager))]
    [HttpPost]
    public async Task<ActionResult<PurchaseResponse>> Add([FromBody] PurchaseRequest request)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new CreatePurchaseCommand(pharmacyId, request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.PurchasingManager))]
    [HttpPut]
    public async Task<ActionResult<PurchaseResponse>> Update(long id, [FromBody] PurchaseRequest request)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new UpdatePurchaseCommand(pharmacyId, id, request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.PurchasingManager))]
    [HttpGet]
    public async Task<ActionResult<PurchaseResponse>> GetById(long id)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetPurchaseBuIdQuery(pharmacyId, id));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.PurchasingManager))]
    [HttpGet]
    public async Task<ActionResult<List<PurchaseResponse>>> GetAll(int pageNumber, int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetAllPurchaseQuery(pharmacyId, pageNumber, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.PurchasingManager))]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var pharmacyId = 1;
        var employeeId = 1; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new DeletePurchaseCommand(pharmacyId, employeeId, id));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.PurchasingManager))]
    [HttpPost]
    public async Task<ActionResult<PurchaseItemResponse>> AddItem(long purchaseId,
        PurchaseItemRequest purchaseItemRequest)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new AddItemToPurchaseCommand(pharmacyId, purchaseId, purchaseItemRequest));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.PurchasingManager))]
    [HttpDelete]
    public async Task<ActionResult<PurchaseItemResponse>> RemoveItem(long purchaseId,
        long purchaseItemId)
    {
        var pharmacyId = 1;
        var employeeId = 1; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response =
            await mediator.Send(new RemoveItemFromPurchaseCommand(employeeId, pharmacyId, purchaseId, purchaseItemId));
        return Ok(response);
    }
}