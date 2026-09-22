using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Product.DTos.Request;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Position.AdminPharmacy) + "," + nameof(Position.ManagerPharmacy))]
public class ProductControllerForAdmin(IProductForAdminEndpoint productForAdminEndpoint) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProductForPharmacyResponse>> Add([FromBody] ProductRequest request, long pharmacyId)
    {
        var response = await productForAdminEndpoint.Add(request, pharmacyId);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<ProductForPharmacyResponse>> Update(long id, [FromBody] UpdateProductRequest request,
        long pharmacyId)
    {
        var response = await productForAdminEndpoint.Update(id, request, pharmacyId);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteById(long id, long pharmacyId)
    {
        var response = await productForAdminEndpoint.DeleteById(id, pharmacyId);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductForPharmacyResponse>> GetById(long id, long pharmacyId)
    {
        var response = await productForAdminEndpoint.GetById(id, pharmacyId);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetAll(long pharmacyId, int pageNumber,
        int pageSize)
    {
        var response = await productForAdminEndpoint.GetAll(pharmacyId, pageNumber, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductForPharmacyResponse>> GetByBarcode(long pharmacyId, string barcode)
    {
        var response = await productForAdminEndpoint.GetByBarcode(pharmacyId, barcode);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByName(long pharmacyId, string name,
        int page,
        int pageSize)
    {
        var response = await productForAdminEndpoint.GetByName(pharmacyId, name, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByCategory(long pharmacyId, long categoryId,
        int pageNumber, int pageSize)
    {
        var response =
            await productForAdminEndpoint.GetByCategory(pharmacyId, categoryId, pageNumber, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetOutOfStockAsync(long pharmacyId, int page,
        int pageSize)
    {
        var response = await productForAdminEndpoint.GetOutOfStock(pharmacyId, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetLowOfStockAsync(long pharmacyId,
        int minimumQuantity, int page,
        int pageSize)
    {
        var response = await productForAdminEndpoint.GetLowOfStock(pharmacyId, minimumQuantity, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByPurchasePriceAsync(long pharmacyId,
        decimal price, int page,
        int pageSize)
    {
        var response = await productForAdminEndpoint.GetByPurchasePrice(pharmacyId, price, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByOrderPrice(long pharmacyId, decimal price,
        int page,
        int pageSize)
    {
        var response = await productForAdminEndpoint.GetByOrderPrice(pharmacyId, price, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByCountry(long pharmacyId, CountryEnum country,
        int page,
        int pageSize)
    {
        var response = await productForAdminEndpoint.GetByCountry(pharmacyId, country, page, pageSize);
        return Ok(response);
    }
}