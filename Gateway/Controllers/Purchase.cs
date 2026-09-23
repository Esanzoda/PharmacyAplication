using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
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
    public async Task<ActionResult<PurchaseResponse>> Add([FromBody] PurchaseRequest request, long pharmacyId)
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
    public async Task<ActionResult<List<PurchaseResponse>>> GetAll(long pharmacyId, int pageNumber, int pageSize)
    {
        var response = await purchaseEndpoint.GetAll(pharmacyId, pageNumber, pageSize);
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