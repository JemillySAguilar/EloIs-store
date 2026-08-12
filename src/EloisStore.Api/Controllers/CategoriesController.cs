using Microsoft.AspNetCore.Authorization;
using EloisStore.Api.Models.Catalog;
using EloisStore.Api.Services.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace EloisStore.Api.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoriesController(CategoryService categoriesLogicaDoService) : ControllerBase
{
    [HttpGet]
    public Task<List<Category>> List() => categoriesLogicaDoService.ListAsync();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public Task<Category> Create(Category category) => categoriesLogicaDoService.CreateAsync(category);

    [HttpPatch]
    [Authorize(Roles = "Admin")]
    public Task<Category> Edit (Category categoryVemDoController) { 
       var categoryUpdate = categoriesLogicaDoService.EditAsync(categoryVemDoController);

       return categoryUpdate;
    }

}
