using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Deliver;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class Supplier(ISupplier supplier) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<SupplierResponse>> Add(
        SupplierRequest request)
    {
        var response = await supplier.Add(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<SupplierResponse>> Update(long id,
        UpdateSupplierRequest request)
    {
        var response = await supplier.Update(id, request);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> Delete(long id)
    {
        var response = await supplier.Delete(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<SupplierResponse>>> Get(
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await supplier.Get(paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<SupplierResponse>> GetById(long id)
    {
        var response = await supplier.GetById(id);
        return Ok(response);
    }
}