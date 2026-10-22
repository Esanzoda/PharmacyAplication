using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Purchase.Commands;
using Pharmacy.CQRS.Purchase.Queries;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Request;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PurchaseController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PurchaseResponse>> Add(long employeeId, [FromBody] PurchaseRequest request,
        long pharmacyId)
    {
        var response = await mediator.Send(new CreatePurchaseCommand(pharmacyId, employeeId, request));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<PurchaseResponse>> GetById(long id, long pharmacyId)
    {
        var response = await mediator.Send(new GetPurchaseBuIdQuery(pharmacyId, id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseResponse>>> GetAll(long pharmacyId, int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetAllPurchaseQuery(pharmacyId, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseResponse>>> GetPurchasesBySupplierIdForSpecificDate(long supplierId,
        long pharmacyId,
        [FromQuery] DateOnly dateOnly,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await mediator.Send(
                new GetPurchasesBySupplierIdForSpecificDateQuery(supplierId, pharmacyId, dateOnly, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseItemResponse>>> GetPurchaseItemByPurchaseId(long supplierId,
        long pharmacyId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await mediator.Send(
                new GetPurchaseItemByPurchaseIdQuery(supplierId, pharmacyId, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseResponse>>> GetPurchasesByDate(long pharmacyId,
        DateOnly date,
        PaginationRequest paginationRequest)
    {
        var response =
            await mediator.Send(
                new GetPurchaseByDateQuery(pharmacyId, date, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseItemResponse>>> GetPurchaseByEmployeeId(long pharmacyId,
        int employeeId,
        int page,
        int pageSize)
    {
        var response =
            await mediator.Send(
                new GetPurchaseByEmployeeIdQuery(pharmacyId, employeeId, page, pageSize));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteById(long employeeId, long id, long pharmacyId)
    {
        var response = await mediator.Send(new DeletePurchaseCommand(pharmacyId, employeeId, id));
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseResponse>> AddItem(long pharmacyId, long purchaseId,
        PurchaseItemRequest purchaseItemRequest)
    {
        var response = await mediator.Send(new AddItemToPurchaseCommand(pharmacyId, purchaseId, purchaseItemRequest));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<PurchaseResponse>> RemoveItem(long employeeId, long purchaseId, long pharmacyId,
        long purchaseItemId)
    {
        var response =
            await mediator.Send(new RemoveItemFromPurchaseCommand(employeeId, pharmacyId, purchaseId, purchaseItemId));
        return Ok(response);
    }
}