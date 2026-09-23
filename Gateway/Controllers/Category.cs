using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Category.DTOs.Request;
using Pharmacy.Domain.Models.Category.DTOs.Response;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Admin))]
public class Category(ICategoryEndpoint categoryEndpoint) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Add([FromBody] CreateCategoryRequest request)
    {
        var response = await categoryEndpoint.Add(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<CategoryResponse>> Update(long id, [FromBody] UpdateCategoryRequest request)
    {
        var response = await categoryEndpoint.Update(id, request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CategoryResponse>> GetById(long id,
        CancellationToken cancellationToken = default)
    {
        var response = await categoryEndpoint.GetById(id, cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAll(int page, int pageSize)
    {
        var response = await categoryEndpoint.GetAll(page, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteById(long id)
    {
        var response = await categoryEndpoint.DeleteById(id);
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetProducts(int categoryId, int page,
        int pageSize)
    {
        var response = await categoryEndpoint.GetProducts(categoryId, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetByName(string name)
    {
        var response = await categoryEndpoint.GetByName(name);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetByStatus(CategoryStatus categoryStatus, int pageNumber,
        int pageSize)
    {
        var response = await categoryEndpoint.GetByStatus(categoryStatus, pageNumber, pageSize);
        return Ok(response);
    }
}