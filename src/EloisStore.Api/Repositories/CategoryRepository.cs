using EloisStore.Api.Data;
using EloisStore.Api.Models.Catalog;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Repositories;

public sealed class CategoryRepository(EloisStoreDbContext dbContext)
{
    public Task<List<Category>> ListAsync() => dbContext.Categories.OrderBy(category => category.Name).ToListAsync();

    public async Task<Category> AddAsync(Category category)
    {
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();
        return category;
    }
}
