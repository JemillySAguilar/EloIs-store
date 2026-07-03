using EloisStore.Api.Data;
using EloisStore.Api.Models.Catalog;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Repositories;

public sealed class ProductRepository(EloisStoreDbContext dbContext)
{
    public Task<List<Product>> ListAsync() =>
        dbContext.Products.Include(product => product.Variants).OrderBy(product => product.Name).ToListAsync();

    public Task<Product?> FindAsync(Guid id) =>
        dbContext.Products.Include(product => product.Variants).FirstOrDefaultAsync(product => product.Id == id);

    public Task<List<Product>> SearchAsync(string term) =>
        dbContext.Products
            .Include(product => product.Variants)
            .Where(product => product.Name.Contains(term) || product.Description.Contains(term))
            .OrderBy(product => product.Name)
            .ToListAsync();

    public async Task<Product> AddAsync(Product product)
    {
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        return product;
    }
}
