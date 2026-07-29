using EloisStore.Api.Models.Catalog;
using EloisStore.Api.Repositories;

namespace EloisStore.Api.Services.Catalog;

public sealed class CategoryService(CategoryRepository classeRepositoryCategoria)
{
    public Task<List<Category>> ListAsync() => classeRepositoryCategoria.ListAsync();

    public Task<Category> CreateAsync(Category category) => classeRepositoryCategoria.AddAsync(category);

    public async Task<Category> EditAsync (Category categoryVemDoController)
    {
        var checkIfExistsResult = await classeRepositoryCategoria.CheckIfExistsByNameAsync(categoryVemDoController.Name, categoryVemDoController.Id);
        if (checkIfExistsResult)
        {
            throw new Exception("Já existe uma categoria com esse nome");
        }

        var checkIfExistSlugResult = await classeRepositoryCategoria.CheckIfExistsBySlugAsync(categoryVemDoController.Slug, categoryVemDoController.Id);
        if (checkIfExistSlugResult)
        {
            throw new Exception("Já existe uma categoria com esse Slug");
        }

        var categoryEditadoNoBanco = await classeRepositoryCategoria.EditAsync(categoryVemDoController);
        return categoryEditadoNoBanco;
    }
}
