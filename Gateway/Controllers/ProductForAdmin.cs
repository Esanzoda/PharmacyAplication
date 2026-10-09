using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Product.DTos.Request;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Product.Endpoint.Clients.IProductApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Position.AdminPharmacy) + "," + nameof(Position.ManagerPharmacy))]
public class ProductForAdmin(IProductForPharmacy productForAdminEndpoint) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProductForPharmacyResponse>> Add(
        [FromBody] ProductRequest request, long pharmacyId)
    {
        var response = await productForAdminEndpoint.Add( pharmacyId,request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<ProductForPharmacyResponse>> Update(
        long id,
        [FromBody] UpdateProductRequest request,
        long pharmacyId)
    {
        var response = await productForAdminEndpoint.Update(pharmacyId,id, request);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteById(long id, long pharmacyId)
    {
        var response = await productForAdminEndpoint.DeleteById(id, pharmacyId);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductForPharmacyResponse>> GetById(
        long id,
        long pharmacyId)
    {
        var response = await productForAdminEndpoint.GetById(id, pharmacyId);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetAll(
        long pharmacyId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await productForAdminEndpoint.GetAll(pharmacyId, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductForPharmacyResponse>> GetByBarcode(
        long pharmacyId,
        string barcode)
    {
        var response = await productForAdminEndpoint.GetByBarcode(pharmacyId, barcode);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByName(
        long pharmacyId,
        string name,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await productForAdminEndpoint.GetByName(pharmacyId, name, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByCategory(
        long pharmacyId,
        long categoryId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await productForAdminEndpoint.GetByCategory(pharmacyId, categoryId, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetOutOfStockAsync(
        long pharmacyId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await productForAdminEndpoint.GetOutOfStock(pharmacyId, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetLowOfStockAsync(
        long pharmacyId,
        int minimumQuantity,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await productForAdminEndpoint.GetLowOfStock(pharmacyId, minimumQuantity, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByPurchasePriceAsync(
        long pharmacyId,
        decimal price,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await productForAdminEndpoint.GetByPurchasePrice(pharmacyId, price, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByOrderPrice(
        long pharmacyId,
        decimal price,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await productForAdminEndpoint.GetByOrderPrice(pharmacyId, price, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByCountry(
        long pharmacyId,
        CountryEnum country,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await productForAdminEndpoint.GetByCountry(pharmacyId, country, paginationRequest);
        return Ok(response);
    }
}