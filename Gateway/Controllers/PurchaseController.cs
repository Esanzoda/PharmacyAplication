using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Request;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Response;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Position.ManagerPharmacy))]
public class PurchaseController(IPurchaseEndpoint purchaseEndpoint) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PurchaseResponse>> Add(long pharmacyId,
        [FromBody] PurchaseRequest request)
    {
        var employeeId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await purchaseEndpoint.Add(employeeId, request, pharmacyId);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<PurchaseResponse>> GetById(long id, long pharmacyId)
    {
        var response = await purchaseEndpoint.GetById(pharmacyId, id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseResponse>>> GetAll(long pharmacyId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await purchaseEndpoint.GetAll(pharmacyId, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseResponse>>> GetPurchasesBySupplierIdForSpecificDate(
        long supplierId,
        long pharmacyId,
        [FromQuery] DateOnly dateOnly,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await purchaseEndpoint.GetPurchasesBySupplierIdForSpecificDate(
                supplierId, pharmacyId, dateOnly, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseItemResponse>>> GetPurchaseItemByPurchaseId(
        long purchaseId,
        long pharmacyId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await purchaseEndpoint.GetPurchaseItemByPurchaseId(
                purchaseId, pharmacyId, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseResponse>>> GetPurchasesByDate(
        long pharmacyId,
        [FromQuery] DateOnly dateOnly,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await purchaseEndpoint.GetPurchasesByDate(
                pharmacyId, dateOnly, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseResponse>>> GetPurchasesBySupplierId(
        long supplierId,
        long pharmacyId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await purchaseEndpoint.GetPurchasesBySupplierId(supplierId, pharmacyId, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseResponse>>> GetPurchaseByEmployeeId(
        long pharmacyId,
        long employeeId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await purchaseEndpoint.GetPurchaseByEmployeeId(
                pharmacyId, employeeId, paginationRequest);
        return Ok(response);
    }


    [HttpDelete]
    public async Task<ActionResult<string>> DeleteById(long id, long pharmacyId)
    {
        var employeeId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await purchaseEndpoint.DeleteById(employeeId, id, pharmacyId);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseItemResponse>> AddItem(long pharmacyId, long purchaseId,
        PurchaseItemRequest purchaseItemRequest)
    {
        var response = await purchaseEndpoint.AddItem(pharmacyId, purchaseId, purchaseItemRequest);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<PurchaseItemResponse>> RemoveItem(long purchaseId, long pharmacyId,
        long purchaseItemId)
    {
        var employeeId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response =
            await purchaseEndpoint.RemoveItem(employeeId, purchaseId, pharmacyId, purchaseItemId);
        return Ok(response);
    }
}