using EloisStore.Api.Models.Catalog;
using EloisStore.Api.Services.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace EloisStore.Api.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoriesController(CategoryService categories) : ControllerBase
{
    [HttpGet]
    public Task<List<Category>> List() => categories.ListAsync();

    [HttpPost]
    public Task<Category> Create(Category category) => categories.CreateAsync(category);
}
