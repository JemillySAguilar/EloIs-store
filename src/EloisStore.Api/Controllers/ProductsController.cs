using Microsoft.AspNetCore.Authorization;
using EloisStore.Api.Models.Catalog;
using EloisStore.Api.Services.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace EloisStore.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(ProductService products, ProductSearchService search) : ControllerBase
{
    [HttpGet]
    public Task<List<Product>> List() => products.ListAsync();

    [HttpGet("search")]
    public Task<List<Product>> Search([FromQuery] string term = "") => search.SearchAsync(term);

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Product>> Find(Guid id)
    {
        var product = await products.FindAsync(id);
        return product is null ? NotFound() : product;
    }

    [HttpPost]
    [Authorize]
    public Task<Product> Create(CreateProductRequest request) => products.CreateAsync(request);
}
