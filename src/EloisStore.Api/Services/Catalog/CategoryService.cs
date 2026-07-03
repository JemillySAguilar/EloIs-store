using EloisStore.Api.Models.Catalog;
using EloisStore.Api.Repositories;

namespace EloisStore.Api.Services.Catalog;

public sealed class CategoryService(CategoryRepository categories)
{
    public Task<List<Category>> ListAsync() => categories.ListAsync();

    public Task<Category> CreateAsync(Category category) => categories.AddAsync(category);
}
