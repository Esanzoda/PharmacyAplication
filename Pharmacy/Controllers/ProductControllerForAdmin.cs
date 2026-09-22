using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Product.Commands;
using Pharmacy.CQRS.Product.Queries;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Product.DTos.Request;
using Pharmacy.Domain.Models.Product.DTos.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductControllerForAdmin(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProductForPharmacyResponse>> Add([FromBody] ProductRequest request, long pharmacyId)
    {
        var response = await mediator.Send(new CreateProductCommand(pharmacyId, request));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<ProductForPharmacyResponse>> Update(long id, [FromBody] UpdateProductRequest request,
        long pharmacyId)
    {
        var response = await mediator.Send(new UpdateProductCommand(pharmacyId, id, request));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteById(long id, long pharmacyId)
    {
        var response = await mediator.Send(new DeleteProductCommand(pharmacyId, id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductForPharmacyResponse>> GetById(long id, long pharmacyId)
    {
        var response = await mediator.Send(new GetPharmacyLowOfStockQuery(pharmacyId, id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetAll(long pharmacyId, int pageNumber,
        int pageSize)
    {
        var response = await mediator.Send(new GetAllPharmacyProductsQuery(pharmacyId, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductForPharmacyResponse>> GetByBarcode(long pharmacyId, string barcode)
    {
        var response = await mediator.Send(new GetProductByBarcodeQuery(pharmacyId, barcode));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByName(long pharmacyId, string name,
        int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetPharmacyProductsByNameQuery(pharmacyId, name, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByCategory(long pharmacyId, long categoryId,
        int pageNumber, int pageSize)
    {
        var response =
            await mediator.Send(new GetPharmacyProductsByCategoryIdQuery(pharmacyId, categoryId, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetOutOfStock(long pharmacyId, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetOutOfStockQuery(pharmacyId, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetLowOfStock(long pharmacyId,
        int minimumQuantity, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetLowOfStockQuery(pharmacyId, minimumQuantity, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByPurchasePrice(long pharmacyId,
        decimal price, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByPurchasePriceQuery(pharmacyId, price, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByOrderPrice(long pharmacyId, decimal price,
        int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetPharmacyProductsBySalePriceQuery(pharmacyId, price, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByCountry(long pharmacyId, CountryEnum country,
        int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetPharmacyProductsByCountryQuery(pharmacyId, country, page, pageSize));
        return Ok(response);
    }
}