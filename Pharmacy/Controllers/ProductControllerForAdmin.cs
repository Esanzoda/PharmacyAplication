using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Product.Commands;
using Pharmacy.CQRS.Product.ProductModels.DTos.Request;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.CQRS.Product.Queries;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductControllerForAdmin(IMediator mediator) : ControllerBase
{
    // [Authorize(Roles = nameof(Role.Admin) )]
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Add([FromBody] ProductRequest request)
    {
        var pharmacyId = 2;
        var response = await mediator.Send(new CreateProductCommand(pharmacyId, request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin) )]
    [HttpPut]
    public async Task<ActionResult<ProductResponse>> Update(long id, [FromBody] UpdateProductRequest request)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new UpdateProductCommand(pharmacyId, id, request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin) )]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new DeleteProductCommand(pharmacyId, id));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<ProductResponse>> GetById(long id)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetPharmacyLowOfStockQuery(pharmacyId, id));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAll(int pageNumber, int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetAllPharmacyProductsQuery(pharmacyId, pageNumber, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<ProductResponse>> GetByBarcodeAsync(string barcode)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetProductByBarcodeQuery(pharmacyId, barcode));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByName(string name,
        int page,
        int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetPharmacyProductsByNameQuery(pharmacyId, name, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin) )]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByCategory(long categoryId,
        int pageNumber, int pageSize)
    {
        var pharmacyId = 1;
        var response =
            await mediator.Send(new GetPharmacyProductsByCategoryIdQuery(pharmacyId, categoryId, pageNumber, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetOutOfStockAsync(int page, int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetOutOfStockQuery(pharmacyId, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetLowOfStockAsync(int minimumQuantity, int page,
        int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetLowOfStockQuery(pharmacyId, minimumQuantity, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByPurchasePriceAsync(decimal price, int page,
        int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetProductsByPurchasePriceQuery(pharmacyId, price, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByOrderPrice(decimal price,
        int page,
        int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetPharmacyProductsBySalePriceQuery(pharmacyId, price, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByCountry(CountryEnum country,
        int page,
        int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetPharmacyProductsByCountryQuery(pharmacyId, country, page, pageSize));
        return Ok(response);
    }
}