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

    public async Task<bool> CheckIfExistsByNameAsync(string name, Guid id) => await dbContext.Categories.AnyAsync(c => c.Name == name &&c.Id != id );
    public async Task<bool> CheckIfExistsBySlugAsync(string slug, Guid id ) => await dbContext.Categories.AnyAsync(c => c.Slug == slug && c.Id != id );

    public async Task<Category> EditAsync(Category category)
    {
        var edit = dbContext.Categories.FirstOrDefault(c => c.Id == category.Id) ?? new Category();
        
        edit.Name = category.Name;
        edit.Slug = category.Slug;

        await dbContext.SaveChangesAsync();

        return edit;
    }
}

