using EloisStore.Api.Data;
using EloisStore.Api.Models.Catalog;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Repositories;

public sealed class UserRepository(EloisStoreDbContext dbContext)
{
    public Task<List<Category>> ListAsync() => dbContext.CategoriaDoBanco.OrderBy(category => category.Name).ToListAsync();

    public async Task<Category> AddAsync(Category category)
    {
        dbContext.CategoriaDoBanco.Add(category);
        await dbContext.SaveChangesAsync();
        return category;
    }

    public async Task<bool> CheckIfExistsByNameAsync(string name, Guid id) => await dbContext.CategoriaDoBanco.AnyAsync(c => c.Name == name &&c.Id != id );
    public async Task<bool> CheckIfExistsBySlugAsync(string slug, Guid id ) => await dbContext.CategoriaDoBanco.AnyAsync(c => c.Slug == slug && c.Id != id );

    public async Task<Category> EditAsync(Category category)
    {
        var edit = dbContext?.CategoriaDoBanco?.FirstOrDefault(c => c.Id == category.Id);
        
        edit.Name = category.Name;
        edit.Slug = category.Slug;

        await dbContext.SaveChangesAsync();

        return edit;
    }
}

