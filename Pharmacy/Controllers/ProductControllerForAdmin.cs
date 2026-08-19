using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Product.Commands;
using Pharmacy.CQRS.Product.ProductModels.DTos.Request;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response.Customer;
using Pharmacy.CQRS.Product.Queries;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Position.AdminPharmacy) + "," + nameof(Position.ManagerPharmacy))]
public class ProductControllerForAdmin(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProductForCustomerResponse>> Add([FromBody] ProductRequest request)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new CreateProductCommand(pharmacyId, request));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<ProductForCustomerResponse>> Update(long id, [FromBody] UpdateProductRequest request)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new UpdateProductCommand(pharmacyId, id, request));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new DeleteProductCommand(pharmacyId, id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductForCustomerResponse>> GetById(long id)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetPharmacyLowOfStockQuery(pharmacyId, id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetAll(int pageNumber, int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetAllPharmacyProductsQuery(pharmacyId, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductForCustomerResponse>> GetByBarcodeAsync(string barcode)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetProductByBarcodeQuery(pharmacyId, barcode));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByName(string name,
        int page,
        int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetPharmacyProductsByNameQuery(pharmacyId, name, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByCategory(long categoryId,
        int pageNumber, int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response =
            await mediator.Send(new GetPharmacyProductsByCategoryIdQuery(pharmacyId, categoryId, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetOutOfStockAsync(int page, int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetOutOfStockQuery(pharmacyId, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetLowOfStockAsync(int minimumQuantity, int page,
        int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetLowOfStockQuery(pharmacyId, minimumQuantity, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByPurchasePriceAsync(decimal price, int page,
        int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetProductsByPurchasePriceQuery(pharmacyId, price, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByOrderPrice(decimal price,
        int page,
        int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetPharmacyProductsBySalePriceQuery(pharmacyId, price, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByCountry(CountryEnum country,
        int page,
        int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetPharmacyProductsByCountryQuery(pharmacyId, country, page, pageSize));
        return Ok(response);
    }
}